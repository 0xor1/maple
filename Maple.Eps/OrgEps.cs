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
            Ep<Create, Org>.DbTx<MapleDb>(
                OrgRpcs.Create,
                async (ctx, db, ses, req) =>
                {
                    var activeOrgs = await db.OrgMembers.CountAsync(x => x.Id == ses.Id, ctx.Ctkn);
                    ctx.ErrorIf(
                        activeOrgs > MaxActiveOrgs,
                        S.OrgTooMany,
                        null,
                        HttpStatusCode.BadRequest
                    );
                    var newOrg = new Db.Org()
                    {
                        Id = Id.New(),
                        Name = req.Name,
                        CreatedOn = DateTimeExt.UtcNowMilli()
                    };
                    await db.Orgs.AddAsync(newOrg, ctx.Ctkn);
                    var m = new Db.OrgMember()
                    {
                        Org = newOrg.Id,
                        Id = ses.Id,
                        Name = req.OwnerMemberName,
                        Role = OrgMemberRole.Owner
                    };
                    await db.OrgMembers.AddAsync(m, ctx.Ctkn);
                    return newOrg.ToApi(m);
                }
            ),
            new Ep<Exact, Org>(
                OrgRpcs.GetOne,
                async (ctx, req) =>
                {
                    var ses = ctx.GetSession();
                    var db = ctx.Get<MapleDb>();
                    var org = await db.Orgs.SingleOrDefaultAsync(x => x.Id == req.Id, ctx.Ctkn);
                    ctx.NotFoundIf(org == null, model: new { Name = "Org" });
                    var m = await db.OrgMembers.SingleOrDefaultAsync(
                        x => x.Org == req.Id && x.Id == ses.Id,
                        ctx.Ctkn
                    );
                    return org.NotNull().ToApi(m);
                }
            ),
            new Ep<Get, IReadOnlyList<Org>>(
                OrgRpcs.Get,
                async (ctx, req) =>
                {
                    var ses = ctx.GetAuthedSession();
                    var db = ctx.Get<MapleDb>();
                    var ms = await db.OrgMembers.Where(x => x.Id == ses.Id).ToListAsync(ctx.Ctkn);
                    var oIds = ms.Select(x => x.Org);
                    var qry = db.Orgs.Where(x => oIds.Contains(x.Id));
                    qry = req switch
                    {
                        (OrgOrderBy.Name, true) => qry.OrderBy(x => x.Name),
                        (OrgOrderBy.CreatedOn, true) => qry.OrderBy(x => x.CreatedOn),
                        (OrgOrderBy.Name, false) => qry.OrderByDescending(x => x.Name),
                        (OrgOrderBy.CreatedOn, false) => qry.OrderByDescending(x => x.CreatedOn),
                    };
                    var os = await qry.ToListAsync(ctx.Ctkn);
                    return os.Select(x => x.ToApi(ms.Single(y => y.Org == x.Id))).ToList();
                }
            ),
            Ep<Update, Org>.DbTx<MapleDb>(
                OrgRpcs.Update,
                async (ctx, db, ses, req) =>
                {
                    var m = await db.OrgMembers.SingleOrDefaultAsync(
                        x => x.Org == req.Id && x.Id == ses.Id,
                        ctx.Ctkn
                    );
                    ctx.InsufficientPermissionsIf(m?.Role != OrgMemberRole.Owner);
                    var org = await db.Orgs.SingleAsync(x => x.Id == req.Id, ctx.Ctkn);
                    org.Name = req.Name;
                    return org.ToApi(m);
                }
            ),
            Ep<Exact, Nothing>.DbTx<MapleDb>(
                OrgRpcs.Delete,
                async (ctx, db, ses, req) =>
                {
                    await EpsUtil.MustHaveOrgAccess(ctx, db, ses.Id, req.Id, OrgMemberRole.Owner);
                    await RawBatchDelete(ctx, db, new List<string>() { req.Id });
                    return Nothing.Inst;
                }
            )
        };

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
