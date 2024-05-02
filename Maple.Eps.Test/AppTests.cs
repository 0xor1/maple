using Common.Server.Test;
using Maple.Db;
using S = Maple.I18n.S;

namespace Maple.Eps.Test;

public class AppTests : IDisposable
{
    private readonly RpcTestRig<MapleDb, Api.Api> Rig;

    public AppTests()
    {
        Rig = new RpcTestRig<MapleDb, Api.Api>(S.Inst, MapleEps.Eps, c => new Api.Api(c));
    }

    [Fact]
    public async void GetConfig_Success()
    {
        var (ali, _, _) = await Rig.NewApi("ali");
        var c = await ali.App.GetConfig();
        Assert.True(c.DemoMode);
        Assert.Equal("https://github.com/0xor1/maple", c.RepoUrl);
    }

    public void Dispose()
    {
        Rig.Dispose();
    }
}
