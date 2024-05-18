using Common.Server;
using Common.Server.Auth;
using Common.Shared;
using Common.Shared.Auth;
using Maple.Api.OrgMember;
using Maple.Db;
using Microsoft.EntityFrameworkCore;
using OrgMember = Maple.Api.OrgMember.OrgMember;
using S = Maple.I18n.S;

namespace Maple.Eps;

internal static class OrgMemberEps
{
    public static IReadOnlyList<IEp> Eps { get; } =
        new List<IEp>()
        {
            Ep<Invite, OrgMember>.DbTx<MapleDb>(OrgMemberRpcs.Invite, Invite),
            Ep<Get, List<OrgMember>>.DbTx<MapleDb>(OrgMemberRpcs.Get, Get),
            Ep<Update, OrgMember>.DbTx<MapleDb>(OrgMemberRpcs.Update, Update),
            Ep<UploadImage, OrgMember>.DbTx<MapleDb>(OrgMemberRpcs.UploadImage, UploadImage),
            Ep<DownloadImage, HasStream>.DbTx<MapleDb>(OrgMemberRpcs.DownloadImage, DownloadImage),
            Ep<Exact, Nothing>.DbTx<MapleDb>(OrgMemberRpcs.Delete, Delete)
        };

    private static async Task<OrgMember> Invite(IRpcCtx ctx, MapleDb db, ISession ses, Invite arg)
    {
        arg = arg with { Email = arg.Email.ToLower() };
        // check current member has sufficient permissions
        var sesOrgMem = await db.OrgMembers
            .Where(x => x.Org == arg.Org && x.Id == ses.Id)
            .SingleOrDefaultAsync(ctx.Ctkn);
        ctx.InsufficientPermissionsIf(sesOrgMem == null);
        var sesRole = sesOrgMem.NotNull().Role;
        ctx.InsufficientPermissionsIf(
            sesRole is not (OrgMemberRole.Owner or OrgMemberRole.Admin)
                || (sesRole is OrgMemberRole.Admin && arg.Role == OrgMemberRole.Owner)
        );
        var (auth, created) = await AuthEps<MapleDb>.CreateAuth(
            ctx,
            db,
            new Register(arg.Email, $"{Crypto.String(16)}a1@"),
            Id.New(),
            ses.Lang,
            ses.DateFmt,
            ses.TimeFmt,
            ses.DateSeparator,
            ses.ThousandsSeparator,
            ses.DecimalSeparator
        );

        var mem = new Db.OrgMember()
        {
            Org = arg.Org,
            Id = auth.Id,
            Name = arg.Name,
            Role = arg.Role,
            Country = arg.Country.Value,
            Data = Json.From(new Data(new(), new("", "", "", false, "", 0, "", null)))
        };
        await db.OrgMembers.AddAsync(mem, ctx.Ctkn);
        if (created)
        {
            var org = await db.Orgs.SingleAsync(x => x.Id == arg.Org, ctx.Ctkn);
            var config = ctx.Get<IConfig>();
            var emailClient = ctx.Get<IEmailClient>();
            var model = new
            {
                BaseHref = config.Auth.BaseHref,
                Email = auth.Email.UrlEncode(),
                Code = auth.VerifyEmailCode,
                OrgName = org.Name,
                InviteeName = arg.Name,
                InvitedByName = sesOrgMem.Name
            };
            await emailClient.SendEmailAsync(
                ctx.String(S.OrgMemberInviteEmailSubject, model),
                ctx.String(S.OrgMemberInviteEmailHtml, model),
                ctx.String(S.OrgMemberInviteEmailText, model),
                config.Email.NoReplyAddress,
                new List<string>() { auth.Email }
            );
        }
        return mem.ToApi();
    }

    private static async Task<List<OrgMember>> Get(
        IRpcCtx ctx,
        MapleDb db,
        ISession ses,
        Get arg
    ) =>
        await db.OrgMembers
            .Where(x => x.Org == arg.Org)
            .Select(x => x.ToApi())
            .ToListAsync(ctx.Ctkn);

    private static async Task<OrgMember> Update(IRpcCtx ctx, MapleDb db, ISession ses, Update arg)
    {
        var sesOrgMem = await db.OrgMembers
            .Where(x => x.Org == arg.Org && x.Id == ses.Id)
            .SingleOrDefaultAsync(ctx.Ctkn);
        // must be a member
        ctx.InsufficientPermissionsIf(sesOrgMem == null);
        var sesRole = sesOrgMem.NotNull().Role;
        // must be an owner or admin, and admins cant make members owners
        ctx.InsufficientPermissionsIf(
            arg.Role < sesRole
                || (ses.Id != arg.Id && sesRole is not (OrgMemberRole.Owner or OrgMemberRole.Admin))
        );
        var updateMem = await db.OrgMembers.SingleOrDefaultAsync(
            x => x.Org == arg.Org && x.Id == arg.Id,
            ctx.Ctkn
        );
        // update target must exist
        ctx.InsufficientPermissionsIf(updateMem == null);
        updateMem.NotNull();
        // cant update a member with high permissions than you
        ctx.InsufficientPermissionsIf(updateMem.Role < sesRole);
        if (updateMem is { Role: OrgMemberRole.Owner } && arg.Role != OrgMemberRole.Owner)
        {
            // a live org owner is being downgraded permissions or being deactivated completely,
            // need to ensure that org is not left without any owners
            var ownerCount = await db.OrgMembers.CountAsync(
                x => x.Org == arg.Org && x.Role == OrgMemberRole.Owner,
                ctx.Ctkn
            );
            ctx.InsufficientPermissionsIf(ownerCount == 1);
        }
        updateMem.Name = arg.Name;
        updateMem.Role = arg.Role;
        updateMem.Country = arg.Country.Value;
        updateMem.Data = Json.From(arg.Data);
        return updateMem.NotNull().ToApi();
    }

    private static async Task<OrgMember> UploadImage(
        IRpcCtx ctx,
        MapleDb db,
        ISession ses,
        UploadImage arg
    )
    {
        var sesOrgMem = await db.OrgMembers
            .Where(x => x.Org == arg.Org && x.Id == ses.Id)
            .SingleOrDefaultAsync(ctx.Ctkn);
        // msut be a member
        ctx.InsufficientPermissionsIf(sesOrgMem == null);
        var sesRole = sesOrgMem.NotNull().Role;
        // must be an owner or admin, and admins cant make members owners
        ctx.InsufficientPermissionsIf(
            ses.Id != arg.Id && sesRole is not (OrgMemberRole.Owner or OrgMemberRole.Admin)
        );
        var updateMem = await db.OrgMembers.SingleOrDefaultAsync(
            x => x.Org == arg.Org && x.Id == arg.Id,
            ctx.Ctkn
        );
        // update target must exist
        ctx.NotFoundIf(updateMem == null);
        updateMem.NotNull();
        // cant update a member with high permissions than you
        ctx.InsufficientPermissionsIf(updateMem.Role < sesRole);
        var store = ctx.Get<IStoreClient>();
        await store.Upload(
            OrgEps.FilesBucket,
            string.Join("/", arg.Org, arg.Id),
            arg.Stream.Type,
            arg.Stream.Size,
            arg.Stream.Data,
            ctx.Ctkn
        );
        var res = updateMem.ToApi();
        res = res with
        {
            Data = res.Data with
            {
                Profile = res.Data.Profile with
                {
                    HasImage = true,
                    ImageSize = arg.Stream.Size,
                    ImageType = arg.Stream.Type
                }
            }
        };
        updateMem.Data = Json.From(res.Data);
        return res;
    }

    private static async Task<HasStream> DownloadImage(
        IRpcCtx ctx,
        MapleDb db,
        ISession ses,
        DownloadImage arg
    )
    {
        var sesOrgMem = await db.OrgMembers
            .Where(x => x.Org == arg.Org && x.Id == ses.Id)
            .SingleOrDefaultAsync(ctx.Ctkn);
        // msut be a member
        ctx.InsufficientPermissionsIf(sesOrgMem == null);
        var mem = await db.OrgMembers.SingleOrDefaultAsync(
            x => x.Org == arg.Org && x.Id == arg.Id,
            ctx.Ctkn
        );
        // update target must exist
        ctx.NotFoundIf(mem == null);
        mem.NotNull();
        var res = mem.ToApi();
        ctx.NotFoundIf(!res.Data.Profile.HasImage);
        var store = ctx.Get<IStoreClient>();
        var data = await store.Download(
            OrgEps.FilesBucket,
            string.Join("/", arg.Org, arg.Id),
            ctx.Ctkn
        );
        return new HasStream()
        {
            Stream = new RpcStream(
                data,
                "profile_image",
                res.Data.Profile.ImageType,
                arg.IsDownload,
                res.Data.Profile.ImageSize
            )
        };
    }

    private static async Task<Nothing> Delete(IRpcCtx ctx, MapleDb db, ISession ses, Exact arg)
    {
        var sesOrgMem = await db.OrgMembers
            .Where(x => x.Org == arg.Org && x.Id == ses.Id)
            .SingleOrDefaultAsync(ctx.Ctkn);
        // must be a member
        ctx.InsufficientPermissionsIf(sesOrgMem == null);
        var sesRole = sesOrgMem.NotNull().Role;
        // must be an owner or admin, and admins cant make members owners
        ctx.InsufficientPermissionsIf(sesRole is not (OrgMemberRole.Owner or OrgMemberRole.Admin));
        var removeMem = await db.OrgMembers.SingleOrDefaultAsync(
            x => x.Org == arg.Org && x.Id == arg.Id,
            ctx.Ctkn
        );
        // update target must exist
        ctx.InsufficientPermissionsIf(removeMem == null);
        removeMem.NotNull();
        // cant update a member with high permissions than you
        ctx.InsufficientPermissionsIf(removeMem.Role < sesRole);
        if (removeMem is { Role: OrgMemberRole.Owner })
        {
            // a live org owner is being downgraded permissions or being deactivated completely,
            // need to ensure that org is not left without any owners
            var ownerCount = await db.OrgMembers.CountAsync(
                x => x.Org == arg.Org && x.Role == OrgMemberRole.Owner,
                ctx.Ctkn
            );
            ctx.InsufficientPermissionsIf(ownerCount == 1);
        }

        await db.OrgMembers
            .Where(x => x.Org == arg.Org && x.Id == arg.Id)
            .ExecuteDeleteAsync(ctx.Ctkn);
        return Nothing.Inst;
    }
}
