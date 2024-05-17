using System.Net;
using Amazon.S3;
using Common.Server;
using Common.Shared;
using Common.Shared.Auth;
using Ganss.Xss;
using Maple.Api.Org;
using Maple.Api.OrgMember;
using Maple.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Data = Maple.Api.Org.Data;
using Exact = Maple.Api.Org.Exact;
using Get = Maple.Api.Org.Get;
using Org = Maple.Api.Org.Org;
using S = Maple.I18n.S;
using Task = System.Threading.Tasks.Task;
using Update = Maple.Api.Org.Update;

namespace Maple.Eps;

public static class OrgEps
{
    private const int MaxActiveOrgs = 10;
    public const string FilesBucket = "mapleuserimages";

    public static void AddServices(IServiceCollection sc)
    {
        sc.AddSingleton<IHtmlSanitizer, HtmlSanitizer>();
    }

    public static async Task InitApp(IServiceProvider sp)
    {
        using var sc = sp.GetRequiredService<IStoreClient>();
        await sc.CreateBucket(FilesBucket, S3CannedACL.Private);
    }

    public static IReadOnlyList<IEp> Eps { get; } =
        new List<IEp>()
        {
            Ep<Create, Org>.DbTx<MapleDb>(OrgRpcs.Create, Create),
            new Ep<Exact, Org>(OrgRpcs.GetOne, GetOne),
            new Ep<Get, IReadOnlyList<Org>>(OrgRpcs.Get, Get),
            Ep<Update, Org>.DbTx<MapleDb>(OrgRpcs.Update, Update),
            Ep<Exact, Nothing>.DbTx<MapleDb>(OrgRpcs.Delete, Delete)
        };

    private static async Task<Org> Create(IRpcCtx ctx, MapleDb db, ISession ses, Create arg)
    {
        var activeOrgs = await db.OrgMembers.CountAsync(x => x.Id == ses.Id, ctx.Ctkn);
        ctx.ErrorIf(activeOrgs > MaxActiveOrgs, S.OrgTooMany, null, HttpStatusCode.BadRequest);
        // TODO html should be sanitized to stop xss attacks but there is a bug that is stripping out css property white-space
        // which Im currently using to demo the app, so this needs putting back in if this app is ever going to be actually used
        // arg = arg with
        // {
        //     Data = arg.Data with
        //     {
        //         ProfileTemplate = ctx.Get<IHtmlSanitizer>().Sanitize(arg.Data.ProfileTemplate)
        //     }
        // };
        var newOrg = new Db.Org()
        {
            Id = Id.New(),
            Name = arg.Name,
            CreatedOn = DateTimeExt.UtcNowMilli(),
            Data = Json.From(arg.Data)
        };
        await db.Orgs.AddAsync(newOrg, ctx.Ctkn);
        var m = new Db.OrgMember()
        {
            Org = newOrg.Id,
            Id = ses.Id,
            Name = arg.OwnerMemberName,
            Role = OrgMemberRole.Owner,
            Country = arg.OwnerMemberCountry.Value,
            Data = Json.From(new Api.OrgMember.Data(new(), new("", "", "", false, "", 0, "", null)))
        };
        await db.OrgMembers.AddAsync(m, ctx.Ctkn);
        return newOrg.ToApi(m);
    }

    private static async Task<Org> GetOne(IRpcCtx ctx, Exact arg)
    {
        var ses = ctx.GetSession();
        var db = ctx.Get<MapleDb>();
        var org = await db.Orgs.SingleOrDefaultAsync(x => x.Id == arg.Id, ctx.Ctkn);
        ctx.NotFoundIf(org == null, model: new { Name = "Org" });
        var m = await db.OrgMembers.SingleOrDefaultAsync(
            x => x.Org == arg.Id && x.Id == ses.Id,
            ctx.Ctkn
        );
        return org.NotNull().ToApi(m);
    }

    private static async Task<IReadOnlyList<Org>> Get(IRpcCtx ctx, Get arg)
    {
        var ses = ctx.GetAuthedSession();
        var db = ctx.Get<MapleDb>();
        var ms = await db.OrgMembers.Where(x => x.Id == ses.Id).ToListAsync(ctx.Ctkn);
        var oIds = ms.Select(x => x.Org);
        var qry = db.Orgs.Where(x => oIds.Contains(x.Id));
        qry = arg switch
        {
            (OrgOrderBy.Name, true) => qry.OrderBy(x => x.Name),
            (OrgOrderBy.CreatedOn, true) => qry.OrderBy(x => x.CreatedOn),
            (OrgOrderBy.Name, false) => qry.OrderByDescending(x => x.Name),
            (OrgOrderBy.CreatedOn, false) => qry.OrderByDescending(x => x.CreatedOn),
        };
        var os = await qry.ToListAsync(ctx.Ctkn);
        return os.Select(x => x.ToApi(ms.Single(y => y.Org == x.Id))).ToList();
    }

    private static async Task<Org> Update(IRpcCtx ctx, MapleDb db, ISession ses, Update arg)
    {
        var m = await db.OrgMembers.SingleOrDefaultAsync(
            x => x.Org == arg.Id && x.Id == ses.Id,
            ctx.Ctkn
        );
        ctx.InsufficientPermissionsIf(m?.Role != OrgMemberRole.Owner);
        var org = await db.Orgs.SingleAsync(x => x.Id == arg.Id, ctx.Ctkn);
        // TODO html should be sanitized to stop xss attacks but there is a bug that is stripping out css property white-space
        // which Im currently using to demo the app, so this needs putting back in if this app is ever going to be actually used
        // arg = arg with
        // {
        //     Data = arg.Data with
        //     {
        //         ProfileTemplate = ctx.Get<IHtmlSanitizer>().Sanitize(arg.Data.ProfileTemplate)
        //     }
        // };
        org.Name = arg.Name;
        org.Data = Json.From(arg.Data);
        // TODO should work out if any of the data was changed and remove any deleted values from the org members data
        return org.ToApi(m);
    }

    private static async Task<Nothing> Delete(IRpcCtx ctx, MapleDb db, ISession ses, Exact arg)
    {
        await EpsUtil.MustHaveOrgAccess(ctx, db, ses.Id, arg.Id, OrgMemberRole.Owner);
        await RawBatchDelete(ctx, db, new List<string>() { arg.Id });
        return Nothing.Inst;
    }

    public static async Task AuthOnDelete(IRpcCtx ctx, MapleDb db, ISession ses)
    {
        // when a user wants to delete their account entirely,
        var allOwnerOrgs = await db.OrgMembers
            .Where(x => x.Id == ses.Id && x.Role == OrgMemberRole.Owner)
            .Select(x => x.Org)
            .Distinct()
            .ToListAsync(ctx.Ctkn);
        var activeOwnerCounts = await db.OrgMembers
            .Where(x => allOwnerOrgs.Contains(x.Org) && x.Role == OrgMemberRole.Owner)
            .GroupBy(x => x.Org)
            .Select(x => new { Org = x.Key, ActiveOwnerCount = x.Count() })
            .ToListAsync(ctx.Ctkn);
        var orgsWithSoleOwner = activeOwnerCounts
            .Where(x => x.ActiveOwnerCount == 1)
            .Select(x => x.Org)
            .ToList();
        if (orgsWithSoleOwner.Any())
        {
            // we can auto delete all their orgs for which they are the sole owner
            await RawBatchDelete(ctx, db, orgsWithSoleOwner);
        }
        // all remaining orgs user is not the sole owner so just deactivate them.
        await RawBatchDeactivate(ctx, db, ses);
    }

    public static async Task AuthValidateFcmTopic(
        IRpcCtx ctx,
        MapleDb db,
        ISession ses,
        IReadOnlyList<string> topic
    )
    {
        ctx.BadRequestIf(topic.Count != 2);
        await EpsUtil.MustHaveOrgAccess(ctx, db, ses.Id, topic[0], OrgMemberRole.Member);
    }

    private static async Task RawBatchDelete(IRpcCtx ctx, MapleDb db, List<string> orgs)
    {
        await db.Orgs.Where(x => orgs.Contains(x.Id)).ExecuteDeleteAsync(ctx.Ctkn);
        await db.OrgMembers.Where(x => orgs.Contains(x.Org)).ExecuteDeleteAsync(ctx.Ctkn);
        using var store = ctx.Get<IStoreClient>();
        foreach (var org in orgs)
        {
            await store.DeletePrefix(FilesBucket, org, ctx.Ctkn);
        }
    }

    private static async Task RawBatchDeactivate(IRpcCtx ctx, MapleDb db, ISession ses)
    {
        await db.OrgMembers.Where(x => x.Id == ses.Id).ExecuteDeleteAsync(ctx.Ctkn);
    }

    public static async Task AuthOnActivation(IRpcCtx ctx, MapleDb db, string id, string email)
    {
        await Task.CompletedTask;
    }
}
