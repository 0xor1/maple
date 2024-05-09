using Common.Shared;

namespace Maple.Api.OrgMember;

public interface IOrgMemberApi
{
    public Task<OrgMember> Invite(Invite arg, CancellationToken ctkn = default);
    public Task<List<OrgMember>> Get(Get arg, CancellationToken ctkn = default);
    public Task<OrgMember> Update(Update arg, CancellationToken ctkn = default);
    public Task Delete(Exact arg, CancellationToken ctkn = default);
}

public class OrgMemberApi : IOrgMemberApi
{
    private readonly IRpcClient _client;

    public OrgMemberApi(IRpcClient client)
    {
        _client = client;
    }

    public Task<OrgMember> Invite(Invite arg, CancellationToken ctkn = default) =>
        _client.Do(OrgMemberRpcs.Invite, arg, ctkn);

    public Task<List<OrgMember>> Get(Get arg, CancellationToken ctkn = default) =>
        _client.Do(OrgMemberRpcs.Get, arg, ctkn);

    public Task<OrgMember> Update(Update arg, CancellationToken ctkn = default) =>
        _client.Do(OrgMemberRpcs.Update, arg, ctkn);

    public Task Delete(Exact arg, CancellationToken ctkn = default) =>
        _client.Do(OrgMemberRpcs.Delete, arg, ctkn);
}

public static class OrgMemberRpcs
{
    public static readonly Rpc<Invite, OrgMember> Invite = new("/org_member/invite");
    public static readonly Rpc<Get, List<OrgMember>> Get = new("/org_member/get");
    public static readonly Rpc<Update, OrgMember> Update = new("/org_member/update");
    public static readonly Rpc<Exact, Nothing> Delete = new("/org_member/delete");
}

public record OrgMember(
    string Org,
    string Id,
    string Name,
    OrgMemberRole Role,
    Key Country,
    Data Data
);

public record Data(Dictionary<Key, Key> SkillMatrix, Profile Profile);

public record Profile(
    string Title,
    string Body,
    string CsvSkills,
    bool HasImage,
    string GithubUrl,
    string? LinkedInUrl
);

public record Get(string Org);

public record Invite(string Org, string Email, string Name, OrgMemberRole Role, Key Country);

public record Update(
    string Org,
    string Id,
    string Name,
    OrgMemberRole Role,
    Key Country,
    Data Data
);

public record Exact(string Org, string Id);

public enum OrgMemberRole
{
    Owner,
    Admin,
    Member
}

public enum OrgMemberOrderBy
{
    Role,
    Name
}
