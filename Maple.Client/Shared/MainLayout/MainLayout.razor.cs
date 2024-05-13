using System.Text.RegularExpressions;

namespace Maple.Client.Shared.MainLayout;

public partial class MainLayout
{
    [GeneratedRegex(@"/org/([^/\s]+)")]
    private static partial Regex OrgIdRx();

    [GeneratedRegex(@"/member/([^/\s]+)")]
    private static partial Regex MemberIdRx();
}
