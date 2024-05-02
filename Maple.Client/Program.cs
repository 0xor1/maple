using Common.Client;
using Maple.Api;
using Maple.Client;
using Maple.I18n;

await Client.Run<App, IApi>(args, S.Inst, (client) => new Api(client));
