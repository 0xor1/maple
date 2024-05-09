using Common.Client;
using Ganss.Xss;
using Maple.Api;
using Maple.Client;
using Maple.Client.Lib;
using Maple.I18n;

await Client.Run<App, IApi>(
    args,
    S.Inst,
    (client) => new Api(client),
    (sc) =>
    {
        sc.AddSingleton<UiCtx>();
        sc.AddSingleton<IHtmlSanitizer, HtmlSanitizer>();
    }
);
