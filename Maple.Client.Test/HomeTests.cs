using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;

namespace Maple.Client.Test;

public class HomeTests : TestBase
{
    [Fact]
    public async Task Anon_Success()
    {
        var ali = await NewTestPack("ali");
        var navMan = ali.Ctx.Services.GetRequiredService<FakeNavigationManager>();
        var aliSes = await ali.Api.Auth.SignOut();
        var aliUi = ali.Ctx.RenderComponent<Maple.Client.Shared.Pages.Home>(
            ps => ps.Add(p => p.Session, aliSes)
        );
        var aliSignInBtn = aliUi.Find(".goto-sign-in");
        var aliRegBtn = aliUi.Find(".goto-register");
        aliSignInBtn.Click(new MouseEventArgs());
        Assert.EndsWith("/cmn/auth/sign_in", navMan.Uri);
        aliRegBtn.Click(new MouseEventArgs());
        Assert.EndsWith("/cmn/auth/register", navMan.Uri);
        ali.Ctx.DisposeComponents();
    }

    [Fact]
    public async Task Authed_Success()
    {
        var ali = await NewTestPack("ali");
        var navMan = ali.Ctx.Services.GetRequiredService<FakeNavigationManager>();
        var aliSes = await ali.Api.Auth.GetSession();
        var aliUi = ali.Ctx.RenderComponent<Maple.Client.Shared.Pages.Home>(
            ps => ps.Add(p => p.Session, aliSes)
        );
        var aliMyCounterBtn = aliUi.Find(".goto-my-orgs");
        aliMyCounterBtn.Click(new MouseEventArgs());
        Assert.EndsWith($"/my_orgs", navMan.Uri);
        ali.Ctx.DisposeComponents();
    }
}
