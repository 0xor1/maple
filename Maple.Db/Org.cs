using Common.Shared;
using Maple.Api.Org;
using Microsoft.EntityFrameworkCore;
using ApiOrg = Maple.Api.Org.Org;

namespace Maple.Db;

[PrimaryKey(nameof(Id))]
public class Org
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedOn { get; set; }
    public string Data { get; set; }

    public ApiOrg ToApi(OrgMember? m) => new(Id, Name, CreatedOn, Json.To<Data>(Data), m?.ToApi());
}
