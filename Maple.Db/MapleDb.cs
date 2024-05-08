using Common.Server.Auth;
using Microsoft.EntityFrameworkCore;

namespace Maple.Db;

public class MapleDb : DbContext, IAuthDb
{
    public MapleDb(DbContextOptions<MapleDb> opts)
        : base(opts) { }

    public DbSet<Auth> Auths { get; set; } = null!;

    public DbSet<FcmReg> FcmRegs { get; set; } = null!;
    public DbSet<Org> Orgs { get; set; } = null!;
    public DbSet<OrgMember> OrgMembers { get; set; } = null!;
}
