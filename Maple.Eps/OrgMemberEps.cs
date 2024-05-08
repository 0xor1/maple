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
            Ep<Invite, OrgMember>.DbTx<MapleDb>(
                OrgMemberRpcs.Invite,
                async (ctx, db, ses, req) =>
                {
                    req = req with { Email = req.Email.ToLower() };
                    // check current member has sufficient permissions
                    var sesOrgMem = await db.OrgMembers
                        .Where(x => x.Org == req.Org && x.Id == ses.Id)
                        .SingleOrDefaultAsync(ctx.Ctkn);
                    ctx.InsufficientPermissionsIf(sesOrgMem == null);
                    var sesRole = sesOrgMem.NotNull().Role;
                    ctx.InsufficientPermissionsIf(
                        sesRole is not (OrgMemberRole.Owner or OrgMemberRole.Admin)
                            || (sesRole is OrgMemberRole.Admin && req.Role == OrgMemberRole.Owner)
                    );
                    var (auth, created) = await AuthEps<MapleDb>.CreateAuth(
                        ctx,
                        db,
                        new Register(req.Email, $"{Crypto.String(16)}a1@"),
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
                        Org = req.Org,
                        Id = auth.Id,
                        Name = req.Name,
                        Role = req.Role
                    };
                    await db.OrgMembers.AddAsync(mem, ctx.Ctkn);
                    if (created)
                    {
                        var org = await db.Orgs.SingleAsync(x => x.Id == req.Org, ctx.Ctkn);
                        var config = ctx.Get<IConfig>();
                        var emailClient = ctx.Get<IEmailClient>();
                        var model = new
                        {
                            BaseHref = config.Server.Listen,
                            Email = auth.Email,
                            Code = auth.VerifyEmailCode,
                            OrgName = org.Name,
                            InviteeName = req.Name,
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
            ),
            Ep<Get, List<OrgMember>>.DbTx<MapleDb>(
                OrgMemberRpcs.Get,
                async (ctx, db, ses, arg) =>
                    await db.OrgMembers
                        .Where(x => x.Org == arg.Org)
                        .Select(x => x.ToApi())
                        .ToListAsync(ctx.Ctkn)
            ),
            Ep<Update, OrgMember>.DbTx<MapleDb>(
                OrgMemberRpcs.Update,
                async (ctx, db, ses, req) =>
                {
                    // check current member has sufficient permissions
                    var sesOrgMem = await db.OrgMembers
                        .Where(x => x.Org == req.Org && x.Id == ses.Id)
                        .SingleOrDefaultAsync(ctx.Ctkn);
                    // msut be a member
                    ctx.InsufficientPermissionsIf(sesOrgMem == null);
                    var sesRole = sesOrgMem.NotNull().Role;
                    // must be an owner or admin, and admins cant make members owners
                    ctx.InsufficientPermissionsIf(
                        sesRole is not (OrgMemberRole.Owner or OrgMemberRole.Admin)
                            || (sesRole is OrgMemberRole.Admin && req.Role == OrgMemberRole.Owner)
                    );
                    var updateMem = await db.OrgMembers.SingleOrDefaultAsync(
                        x => x.Org == req.Org && x.Id == req.Id,
                        ctx.Ctkn
                    );
                    // update target must exist
                    ctx.InsufficientPermissionsIf(updateMem == null);
                    updateMem.NotNull();
                    // cant update a member with high permissions than you
                    ctx.InsufficientPermissionsIf(updateMem.Role < sesRole);
                    if (
                        updateMem is { Role: OrgMemberRole.Owner }
                        && ((req.Role != null && req.Role != OrgMemberRole.Owner))
                    )
                    {
                        // a live org owner is being downgraded permissions or being deactivated completely,
                        // need to ensure that org is not left without any owners
                        var ownerCount = await db.OrgMembers.CountAsync(
                            x => x.Org == req.Org && x.Role == OrgMemberRole.Owner
                        );
                        ctx.InsufficientPermissionsIf(ownerCount == 1);
                    }
                    updateMem.Name = req.Name ?? updateMem.Name;
                    updateMem.Role = req.Role ?? updateMem.Role;
                    return updateMem.NotNull().ToApi();
                }
            )
        };
}
