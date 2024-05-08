using System.Text.RegularExpressions;
using Common.Server;
using Maple.Api.OrgMember;
using Maple.Db;
using Microsoft.EntityFrameworkCore;
using S = Maple.I18n.S;
using Task = System.Threading.Tasks.Task;

namespace Maple.Eps;

public static class EpsUtil
{
    public static void ValidStr(
        IRpcCtx ctx,
        string val,
        int min,
        int max,
        string name,
        List<Regex>? regexes = null
    )
    {
        var m = new
        {
            Name = name,
            Min = min,
            Max = max,
            Regexes = regexes?.Select(x => x.ToString()).ToList()
        };
        ctx.BadRequestIf(val.Length < min || val.Length > max, S.StringValidation, m);
        if (regexes != null)
        {
            foreach (var r in regexes)
            {
                ctx.BadRequestIf(!r.IsMatch(val), S.StringValidation, m);
            }
        }
    }

    public static async Task<OrgMemberRole?> OrgRole(
        IRpcCtx ctx,
        MapleDb db,
        string user,
        string org
    )
    {
        var orgMem = await db.OrgMembers.SingleOrDefaultAsync(
            x => x.Org == org && x.Id == user,
            ctx.Ctkn
        );
        return orgMem?.Role;
    }

    public static async Task<bool> HasOrgAccess(
        IRpcCtx ctx,
        MapleDb db,
        string user,
        string org,
        OrgMemberRole role
    )
    {
        var memRole = await OrgRole(ctx, db, user, org);
        return memRole != null && memRole <= role;
    }

    public static async Task MustHaveOrgAccess(
        IRpcCtx ctx,
        MapleDb db,
        string user,
        string org,
        OrgMemberRole role
    ) => ctx.InsufficientPermissionsIf(!await HasOrgAccess(ctx, db, user, org, role));
}
