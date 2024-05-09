using Common.Client;
using Maple.Api;
using Maple.Api.Org;
using Maple.Api.OrgMember;

namespace Maple.Client.Lib;

public class UiCtx
{
    private readonly SemaphoreSlim _ss = new(1, 1);
    private readonly IApi _api;
    private readonly IAuthService _auth;

    public string? OrgId { get; private set; }
    public Org? Org { get; private set; }

    public List<OrgMember> OrgMembers { get; private set; } = new();
    public OrgMember? OrgMember { get; private set; }

    public bool HasOrgOwnerPerm => OrgMember?.Role == OrgMemberRole.Owner;

    public bool HasOrgAdminPerm => OrgMember?.Role == OrgMemberRole.Admin;

    public UiCtx(IApi api, IAuthService auth)
    {
        _api = api;
        _auth = auth;
    }

    public async Task Set(Org? org)
    {
        await _ss.WaitAsync();
        try
        {
            // no white space shenanigans
            var orgId = org?.Id;

            var orgChanged = OrgId != orgId;

            OrgId = orgId;
            var ses = (await _auth.GetSession());
            var sesId = ses.Id;

            if (OrgId == null)
            {
                Org = null;
                OrgMember = null;
            }

            if (orgChanged && OrgId != null)
            {
                Org = org ?? await _api.Org.GetOne(new(OrgId));
                OrgMembers = (await _api.OrgMember.Get(new(OrgId)));
                OrgMember = OrgMembers.SingleOrDefault(x => x.Id == ses.Id);
            }
        }
        finally
        {
            _ss.Release();
        }
    }
}
