using Common.Shared;
using MessagePack;

namespace Maple.Api.OrgMember;

public interface IOrgMemberApi
{
    public Task<OrgMember> Invite(Invite arg, CancellationToken ctkn = default);
    public Task<List<OrgMember>> Get(Get arg, CancellationToken ctkn = default);
    public Task<OrgMember> Update(Update arg, CancellationToken ctkn = default);
    public Task<OrgMember> UploadImage(UploadImage arg, CancellationToken ctkn = default);
    public Task<HasStream> DownloadImage(DownloadImage arg, CancellationToken ctkn = default);
    public string DownloadImageUrl(DownloadImage arg);
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

    public Task<OrgMember> UploadImage(UploadImage arg, CancellationToken ctkn = default) =>
        _client.Do(OrgMemberRpcs.UploadImage, arg, ctkn);

    public Task<HasStream> DownloadImage(DownloadImage arg, CancellationToken ctkn = default) =>
        _client.Do(OrgMemberRpcs.DownloadImage, arg, ctkn);

    public string DownloadImageUrl(DownloadImage arg) =>
        _client.GetUrl(OrgMemberRpcs.DownloadImage, arg);

    public Task Delete(Exact arg, CancellationToken ctkn = default) =>
        _client.Do(OrgMemberRpcs.Delete, arg, ctkn);
}

public static class OrgMemberRpcs
{
    public static readonly Rpc<Invite, OrgMember> Invite = new("/org_member/invite");
    public static readonly Rpc<Get, List<OrgMember>> Get = new("/org_member/get");
    public static readonly Rpc<Update, OrgMember> Update = new("/org_member/update");
    public static readonly Rpc<UploadImage, OrgMember> UploadImage =
        new("/org_member/upload_image", 10 * Size.MB);
    public static readonly Rpc<DownloadImage, HasStream> DownloadImage =
        new("/org_member/download_image");
    public static readonly Rpc<Exact, Nothing> Delete = new("/org_member/delete");
}

[MessagePackObject]
public record OrgMember(
    [property: Key(0)] string Org,
    [property: Key(1)] string Id,
    [property: Key(2)] string Name,
    [property: Key(3)] OrgMemberRole Role,
    [property: Key(4)] Key Country,
    [property: Key(5)] Data Data
);

[MessagePackObject]
public record Data(
    [property: Key(0)] Dictionary<string, ExpLevel> SkillMatrix,
    [property: Key(1)] Profile Profile
);

[MessagePackObject]
public record Profile(
    [property: Key(0)] string Title,
    [property: Key(1)] string Body,
    [property: Key(2)] string CsvSkills,
    [property: Key(3)] bool HasImage,
    [property: Key(4)] string ImageType,
    [property: Key(5)] ulong ImageSize,
    [property: Key(6)] string GithubUrl,
    [property: Key(7)] string? LinkedInUrl
);

[MessagePackObject]
public record Get([property: Key(0)] string Org);

[MessagePackObject]
public record Invite(
    [property: Key(0)] string Org,
    [property: Key(1)] string Email,
    [property: Key(2)] string Name,
    [property: Key(3)] OrgMemberRole Role,
    [property: Key(4)] Key Country
);

[MessagePackObject]
public record Update(
    [property: Key(0)] string Org,
    [property: Key(1)] string Id,
    [property: Key(2)] string Name,
    [property: Key(3)] OrgMemberRole Role,
    [property: Key(4)] Key Country,
    [property: Key(5)] Data Data
);

[MessagePackObject]
public record Exact([property: Key(0)] string Org, [property: Key(1)] string Id);

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

public enum ExpLevel
{
    None,
    Low,
    Mid,
    High,
    Expert
}

[MessagePackObject]
public record UploadImage([property: Key(0)] string Org, [property: Key(1)] string Id) : HasStream;

[MessagePackObject]
public record DownloadImage(
    [property: Key(0)] string Org,
    [property: Key(1)] string Id,
    [property: Key(2)] bool IsDownload
);
