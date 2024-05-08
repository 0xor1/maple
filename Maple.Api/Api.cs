using Common.Shared;
using Common.Shared.Auth;
using Maple.Api.Org;
using Maple.Api.OrgMember;

namespace Maple.Api;

public interface IApi : Common.Shared.Auth.IApi
{
    public IOrgApi Org { get; }
    public IOrgMemberApi OrgMember { get; }
}

public class Api : IApi
{
    public Api(IRpcClient client)
    {
        App = new AppApi(client);
        Auth = new AuthApi(client);
        Org = new OrgApi(client);
        OrgMember = new OrgMemberApi(client);
    }

    public IAppApi App { get; }
    public IAuthApi Auth { get; }
    public IOrgApi Org { get; }
    public IOrgMemberApi OrgMember { get; }
}
