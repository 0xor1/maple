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
    public string? OrgMemberId { get; private set; }
    public Org? Org { get; private set; }

    public List<OrgMember> OrgMembers { get; private set; } = new();
    public OrgMember? OrgMember { get; private set; }
    public OrgMember? SesOrgMember { get; private set; }

    public bool HasOrgOwnerPerm => SesOrgMember?.Role == OrgMemberRole.Owner;

    public bool HasOrgAdminPerm => SesOrgMember?.Role <= OrgMemberRole.Admin;

    public UiCtx(IApi api, IAuthService auth)
    {
        _api = api;
        _auth = auth;
    }

    public async Task Set(string? orgId, string? orgMemberId)
    {
        await _ss.WaitAsync();
        try
        {
            OrgId = orgId;
            OrgMemberId = orgMemberId;
            var ses = await _auth.GetSession();
            var sesId = ses.Id;

            if (OrgId == null)
            {
                Org = null;
                OrgMembers = new List<OrgMember>();
                OrgMember = null;
                SesOrgMember = null;
                return;
            }

            Org = await _api.Org.GetOne(new(OrgId));
            OrgMembers = await _api.OrgMember.Get(new(OrgId));
            OrgMembers.AddRange(
                new List<OrgMember>()
                {
                    new(
                        Org.Id,
                        "a",
                        "Andrew Price",
                        OrgMemberRole.Admin,
                        new("algeria"),
                        new(
                            new()
                            {
                                { "Javascript", ExpLevel.None },
                                { "Typescript", ExpLevel.Low },
                                { "AWS", ExpLevel.Mid },
                                { "SQL", ExpLevel.High },
                                { "Mariadb", ExpLevel.Expert }
                            },
                            new("", "", "", false, "", 0, "", null)
                        )
                    ),
                    new(
                        Org.Id,
                        "b",
                        "Benjamin Button",
                        OrgMemberRole.Member,
                        new("italy"),
                        new(
                            new()
                            {
                                { "Javascript", ExpLevel.Expert },
                                { "Typescript", ExpLevel.Expert },
                                { "AWS", ExpLevel.Low },
                                { "SQL", ExpLevel.Low },
                                { "Mariadb", ExpLevel.Expert }
                            },
                            new("", "", "", false, "", 0, "", null)
                        )
                    ),
                    new(
                        Org.Id,
                        "c",
                        "Cat Sanchez",
                        OrgMemberRole.Owner,
                        new("norway"),
                        new(
                            new()
                            {
                                { "Javascript", ExpLevel.High },
                                { "Typescript", ExpLevel.Mid },
                                { "AWS", ExpLevel.Mid },
                                { "SQL", ExpLevel.Low },
                                { "Mariadb", ExpLevel.None }
                            },
                            new("", "", "", false, "", 0, "", null)
                        )
                    )
                }
            );
            OrgMember = OrgMembers.FirstOrDefault(x => x.Id == orgMemberId);
            SesOrgMember = OrgMembers.FirstOrDefault(x => x.Id == sesId);
        }
        finally
        {
            _ss.Release();
        }
    }
}
