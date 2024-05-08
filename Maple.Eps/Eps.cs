using Common.Server;
using Common.Server.Auth;
using Maple.Db;

namespace Maple.Eps;

public static class MapleEps
{
    private static IReadOnlyList<IEp>? _eps;
    public static IReadOnlyList<IEp> Eps
    {
        get
        {
            if (_eps == null)
            {
                var eps =
                    (List<IEp>)
                        new CommonEps<MapleDb>(
                            5,
                            true,
                            5,
                            OrgEps.AuthOnActivation,
                            OrgEps.AuthOnDelete,
                            OrgEps.AuthValidateFcmTopic
                        ).Eps;
                eps.AddRange(OrgEps.Eps);
                eps.AddRange(OrgMemberEps.Eps);
                _eps = eps;
            }

            return _eps;
        }
    }
}
