using Common.Shared;

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

public record Org(
    string Id,
    string Name,
    DateTime CreatedOn,
    Data Data,
    OrgMember.OrgMember? Member
);

public record Data(Dictionary<Key, string> Skills, Dictionary<Key, string> ProficiencyLevels);

public record Create(string Name, string OwnerMemberName, Key OwnerMemberCountry, Data Data);

public record Get(OrgOrderBy OrderBy = OrgOrderBy.Name, bool Asc = true);

public record Update(string Id, string Name, Data Data);

public record Exact(string Id);

public enum OrgOrderBy
{
    Name,
    CreatedOn
}
