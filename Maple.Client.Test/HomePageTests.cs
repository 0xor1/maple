namespace Maple.Client.Test;

public class HomePageTests : TestBase
{
    [Fact]
    public async Task PageWrapper_Success()
    {
        var ali = await NewTestPack("ali");
        ali.Ctx.RenderComponent<Maple.Client.Pages.HomePage>();
        ali.Ctx.DisposeComponents();
    }
}
