using Common.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Maple.Client.Test;

public class MainLayoutTest : TestBase
{
    [Fact]
    public async Task Load_Success()
    {
        var ali = await NewTestPack("ali");
        ali.Ctx.RenderComponent<Maple.Client.Shared.MainLayout.MainLayout>();
        var auth = ali.Ctx.Services.GetRequiredService<IAuthService>();
        await auth.SignOut();
        ali.Ctx.DisposeComponents();
    }

    [Fact]
    public async Task CollapseIfNarrow_Success()
    {
        var ali = await NewTestPack("ali");
        var aliUi = ali.Ctx.RenderComponent<Maple.Client.Shared.MainLayout.MainLayout>();
        ali.Ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        ali.Ctx.JSInterop.Setup<decimal>("getWidth").SetResult(23);
        aliUi.Find(".goto-home").Click();
        ali.Ctx.DisposeComponents();
    }
}
