// Generated Code File, Do Not Edit.
// This file is generated with Common.Cmds.
// see https://github.com/0xor1/common/blob/main/Common.Cmds/I18n.cs
// executed with arguments: i18n <abs_file_path_to>/Maple.I18n Maple.I18n false

using Common.Shared;

namespace Maple.I18n;

public static partial class S
{
    private static readonly Dictionary<string, TemplatableString> EN_Strings =
        new()
        {
            { Counter, new("Counter") },
            { Decrement, new("Decrement") },
            { Decrementing, new("Decrementing") },
            { Home, new("Home") },
            {
                HomeBody,
                new(
                    "<p>Welcome to your new dotnet starter kit.</p><p>You will find:</p><ul><li>Client: a blazor wasm app using radzen ui library</li><li>Server: aspnet with rpc pattern api and entity framework db interface</li></ul>"
                )
            },
            { HomeHeader, new("Hello, Maple!") },
            { Increment, new("Increment") },
            { Incrementing, new("Incrementing") },
            { MyCounter, new("My Counter") },
            {
                OrgMemberInviteEmailHtml,
                new(
                    "<p>Dear <strong>{{InviteeName}}</strong></p><p><strong>{{InvitedByName}}</strong> has invited you to join the organisation: <strong>{{OrgName}}</strong></p><p><a href=\"{{BaseHref}}/verify_email?email={{Email}}&code={{Code}}\">Please click this link to verify your email address and join <strong>{{OrgName}}</strong></a></p>"
                )
            },
            { OrgMemberInviteEmailSubject, new("{{OrgName}} - Project Management Invite") },
            {
                OrgMemberInviteEmailText,
                new(
                    "Dear {{InviteeName}}\n\n{{InvitedByName}} has invited you to join the organisation: {{OrgName}}\n\nPlease click this link to verify your email address and join {{OrgName}}:\n\n{{BaseHref}}/verify_email?email={{Email}}&code={{Code}}"
                )
            },
            { OrgTooMany, new("You are already a member of too many Orgs") },
            {
                StringValidation,
                new("Invalid string {{Name}}, Min {{Min}}, Max {{Max}}, Regexes {{Regexes}}")
            },
        };
}
