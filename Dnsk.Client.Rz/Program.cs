using Common.Client;
using Common.Shared.Auth;
using Dnsk.Api;
using Dnsk.Client.Rz;
using Dnsk.I18n;

await Client.Run<App, Dnsk.Api.IApi>(args, S.Inst, Dnsk.Api.IApi.Init());