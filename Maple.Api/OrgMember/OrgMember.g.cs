// Generated Code File, Do Not Edit.
// This file is generated with Common.Cli.
// see https://github.com/0xor1/common/blob/main/Common.Cli/Api.cs
// executed with arguments: api <abs_file_path_to>/Maple.Api

#nullable enable

using Common.Shared;
using MessagePack;


namespace Maple.Api.OrgMember;

public interface IOrgMemberApi
{
    public Task<OrgMember> Invite(Invite arg, CancellationToken ctkn = default);
    public Task<List<OrgMember>> Get(Get arg, CancellationToken ctkn = default);
    public Task<OrgMember> Update(OrgMember arg, CancellationToken ctkn = default);
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
    
    public Task<OrgMember> Update(OrgMember arg, CancellationToken ctkn = default) =>
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
    public static readonly Rpc<Invite, OrgMember> Invite = new("/orgMember/invite");
    public static readonly Rpc<Get, List<OrgMember>> Get = new("/orgMember/get");
    public static readonly Rpc<OrgMember, OrgMember> Update = new("/orgMember/update");
    public static readonly Rpc<UploadImage, OrgMember> UploadImage = new("/orgMember/uploadImage");
    public static readonly Rpc<DownloadImage, HasStream> DownloadImage = new("/orgMember/downloadImage");
    public static readonly Rpc<Exact, Nothing> Delete = new("/orgMember/delete");
    
}



[MessagePackObject]
public record OrgMember
{
    public OrgMember(
        string org,
        string id,
        string name,
        OrgMemberRole role,
        Key country,
        Data data
        
    )
    {
        Org = org;
        Id = id;
        Name = name;
        Role = role;
        Country = country;
        Data = data;
        
    }
    
    [Key(0)]
    public string Org { get; set; }
    [Key(1)]
    public string Id { get; set; }
    [Key(2)]
    public string Name { get; set; }
    [Key(3)]
    public OrgMemberRole Role { get; set; }
    [Key(4)]
    public Key Country { get; set; }
    [Key(5)]
    public Data Data { get; set; }
    
}



[MessagePackObject]
public record Data
{
    public Data(
        Dictionary<string, ExpLevel> skillMatrix,
        Profile profile
        
    )
    {
        SkillMatrix = skillMatrix;
        Profile = profile;
        
    }
    
    [Key(0)]
    public Dictionary<string, ExpLevel> SkillMatrix { get; set; }
    [Key(1)]
    public Profile Profile { get; set; }
    
}



[MessagePackObject]
public record Profile
{
    public Profile(
        string title,
        string body,
        string csvSkills,
        bool hasImage,
        string imageType,
        ulong imageSize,
        string githubUrl,
        string? linkedInUrl
        
    )
    {
        Title = title;
        Body = body;
        CsvSkills = csvSkills;
        HasImage = hasImage;
        ImageType = imageType;
        ImageSize = imageSize;
        GithubUrl = githubUrl;
        LinkedInUrl = linkedInUrl;
        
    }
    
    [Key(0)]
    public string Title { get; set; }
    [Key(1)]
    public string Body { get; set; }
    [Key(2)]
    public string CsvSkills { get; set; }
    [Key(3)]
    public bool HasImage { get; set; }
    [Key(4)]
    public string ImageType { get; set; }
    [Key(5)]
    public ulong ImageSize { get; set; }
    [Key(6)]
    public string GithubUrl { get; set; }
    [Key(7)]
    public string? LinkedInUrl { get; set; }
    
}



[MessagePackObject]
public record Get
{
    public Get(
        string org
        
    )
    {
        Org = org;
        
    }
    
    [Key(0)]
    public string Org { get; set; }
    
}



[MessagePackObject]
public record Invite
{
    public Invite(
        string org,
        string email,
        string name,
        OrgMemberRole role,
        Key country
        
    )
    {
        Org = org;
        Email = email;
        Name = name;
        Role = role;
        Country = country;
        
    }
    
    [Key(0)]
    public string Org { get; set; }
    [Key(1)]
    public string Email { get; set; }
    [Key(2)]
    public string Name { get; set; }
    [Key(3)]
    public OrgMemberRole Role { get; set; }
    [Key(4)]
    public Key Country { get; set; }
    
}



[MessagePackObject]
public record Exact
{
    public Exact(
        string org,
        string id
        
    )
    {
        Org = org;
        Id = id;
        
    }
    
    [Key(0)]
    public string Org { get; set; }
    [Key(1)]
    public string Id { get; set; }
    
}



[MessagePackObject]
public record UploadImage : HasStream
{
    public UploadImage(
        string org,
        string id
        
    )
    {
        Org = org;
        Id = id;
        
    }
    
    [Key(0)]
    public string Org { get; set; }
    [Key(1)]
    public string Id { get; set; }
    
}



[MessagePackObject]
public record DownloadImage
{
    public DownloadImage(
        string org,
        string id,
        bool isDownload
        
    )
    {
        Org = org;
        Id = id;
        IsDownload = isDownload;
        
    }
    
    [Key(0)]
    public string Org { get; set; }
    [Key(1)]
    public string Id { get; set; }
    [Key(2)]
    public bool IsDownload { get; set; }
    
}




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
