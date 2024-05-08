using Common.Shared;
using Maple.Api.OrgMember;
using Microsoft.EntityFrameworkCore;

namespace Maple.Db;

[PrimaryKey(nameof(Org), nameof(Id))]
public class OrgMember
{
    public string Org { get; set; }
    public string Id { get; set; }
    public string Name { get; set; }
    public OrgMemberRole Role { get; set; }

    public string Data { get; set; }

    public Api.OrgMember.OrgMember ToApi() => new(Org, Id, Name, Role, Json.To<Data>(Data));
}
