using Common.Shared;
using Common.Shared.Auth;
using Maple.Api.Counter;

namespace Maple.Api;

public interface IApi : Common.Shared.Auth.IApi
{
    public ICounterApi Counter { get; }
}

public class Api : IApi
{
    public Api(IRpcClient client)
    {
        App = new AppApi(client);
        Auth = new AuthApi(client);
        Counter = new CounterApi(client);
    }

    public IAppApi App { get; }
    public IAuthApi Auth { get; }
    public ICounterApi Counter { get; }
}
