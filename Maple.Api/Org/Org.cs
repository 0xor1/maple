using Common.Shared;
using MessagePack;

namespace Maple.Api.Org;

public interface IOrgApi
{
    public Task<Org> Create(Create arg, CancellationToken ctkn = default);
    public Task<Org> GetOne(Exact arg, CancellationToken ctkn = default);
    public Task<IReadOnlyList<Org>> Get(Get arg, CancellationToken ctkn = default);
    public Task<Org> Update(Update arg, CancellationToken ctkn = default);
    public System.Threading.Tasks.Task Delete(Exact arg, CancellationToken ctkn = default);
}

public class OrgApi : IOrgApi
{
    private readonly IRpcClient _client;

    public OrgApi(IRpcClient client)
    {
        _client = client;
    }

    public Task<Org> Create(Create arg, CancellationToken ctkn = default) =>
        _client.Do(OrgRpcs.Create, arg, ctkn);

    public Task<Org> GetOne(Exact arg, CancellationToken ctkn = default) =>
        _client.Do(OrgRpcs.GetOne, arg, ctkn);

    public Task<IReadOnlyList<Org>> Get(Get arg, CancellationToken ctkn = default) =>
        _client.Do(OrgRpcs.Get, arg, ctkn);

    public Task<Org> Update(Update arg, CancellationToken ctkn = default) =>
        _client.Do(OrgRpcs.Update, arg, ctkn);

    public Task Delete(Exact arg, CancellationToken ctkn = default) =>
        _client.Do(OrgRpcs.Delete, arg, ctkn);
}

public static class OrgRpcs
{
    public static readonly Rpc<Create, Org> Create = new("/org/create");
    public static readonly Rpc<Exact, Org> GetOne = new("/org/get_one");
    public static readonly Rpc<Get, IReadOnlyList<Org>> Get = new("/org/get");
    public static readonly Rpc<Update, Org> Update = new("/org/update");
    public static readonly Rpc<Exact, Nothing> Delete = new("/org/delete");
}

[MessagePackObject]
public record Org(
    [property: Key(0)] string Id,
    [property: Key(1)] string Name,
    [property: Key(2)] DateTime CreatedOn,
    [property: Key(3)] Data Data,
    [property: Key(4)] OrgMember.OrgMember? Member
);

[MessagePackObject]
public record KeyDisplay([property: Key(0)] Key Key, [property: Key(1)] string Display);

[MessagePackObject]
public record Data(
    [property: Key(0)] List<string> Skills,
    [property: Key(1)] string ProfileTemplate
);

[MessagePackObject]
public record Create(
    [property: Key(0)] string Name,
    [property: Key(1)] string OwnerMemberName,
    [property: Key(2)] Key OwnerMemberCountry,
    [property: Key(3)] Data Data
);

[MessagePackObject]
public record Get(
    [property: Key(0)] OrgOrderBy OrderBy = OrgOrderBy.Name,
    [property: Key(1)] bool Asc = true
);

[MessagePackObject]
public record Update(
    [property: Key(0)] string Id,
    [property: Key(1)] string Name,
    [property: Key(2)] Data Data
);

[MessagePackObject]
public record Exact([property: Key(0)] string Id);

public enum OrgOrderBy
{
    Name,
    CreatedOn
}
