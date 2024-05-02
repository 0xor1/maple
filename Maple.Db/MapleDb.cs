using Common.Server.Auth;
using Microsoft.EntityFrameworkCore;
using ApiCounter = Maple.Api.Counter.Counter;

namespace Maple.Db;

public class MapleDb : DbContext, IAuthDb
{
    public MapleDb(DbContextOptions<MapleDb> opts)
        : base(opts) { }

    public DbSet<Auth> Auths { get; set; } = null!;

    public DbSet<FcmReg> FcmRegs { get; set; } = null!;
    public DbSet<Counter> Counters { get; set; } = null!;
}

[PrimaryKey(nameof(User))]
public class Counter
{
    public string User { get; set; }
    public uint Value { get; set; }

    public ApiCounter ToApi()
    {
        return new ApiCounter(User, Value);
    }
}
