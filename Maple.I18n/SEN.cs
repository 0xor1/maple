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
            { Add, new("Add") },
            { Cancel, new("Cancel") },
            { Confirm, new("Confirm") },
            { Create, new("Create") },
            { CreatedOn, new("Created On") },
            { Delete, new("Delete") },
            { Display, new("Display") },
            { Edit, new("Edit") },
            { Home, new("Home") },
            {
                HomeBody,
                new(
                    "<p>Welcome to Maple.</p><p>Maple is a HR management tool, it tracks skills proficiencies and individual profiles</p>"
                )
            },
            { HomeHeader, new("Hello, Maple!") },
            { Invite, new("Invite") },
            { Key, new("Key") },
            { Loading, new("Loading") },
            { Name, new("Name") },
            { New, new("New") },
            { OnePerLine, new("One per line") },
            {
                OrgConfirmDeleteOrg,
                new(
                    "<p>Are you sure you want to delete the org <strong>{{Name}}</strong>?</p><p>This can not be undone.</p>"
                )
            },
            {
                OrgMemberInviteEmailHtml,
                new(
                    "<p>Dear <strong>{{InviteeName}}</strong></p><p><strong>{{InvitedByName}}</strong> has invited you to join the org: <strong>{{OrgName}}</strong></p><p><a href=\"{{BaseHref}}/verify_email?email={{Email}}&code={{Code}}\">Please click this link to verify your email address and join <strong>{{OrgName}}</strong></a></p>"
                )
            },
            { OrgMemberInviteEmailSubject, new("{{OrgName}} - Project Management Invite") },
            {
                OrgMemberInviteEmailText,
                new(
                    "Dear {{InviteeName}}\n\n{{InvitedByName}} has invited you to join the org: {{OrgName}}\n\nPlease click this link to verify your email address and join {{OrgName}}:\n\n{{BaseHref}}/verify_email?email={{Email}}&code={{Code}}"
                )
            },
            { OrgMemberRole, new("Role") },
            { OrgMemberRoleAdmin, new("Admin") },
            { OrgMemberRoleOwner, new("Owner") },
            { OrgMemberRoleMember, new("Member") },
            { OrgMembers, new("Members") },
            { OrgMyOrgs, new("My Orgs") },
            { OrgName, new("Org Name") },
            { OrgNewMember, new("New Member") },
            { OrgNewOrg, new("New Org") },
            { OrgNoMembers, new("No Members") },
            { OrgNoOrgs, new("No Orgs") },
            { OrgTooMany, new("You are already a member of too many Orgs") },
            { OrgUpdateMember, new("Update Member") },
            { OrgUpdateOrg, new("Update Org") },
            { OrgYourCountry, new("Your Country") },
            { OrgYourName, new("Your Name") },
            { ProficiencyLevels, new("Proficiency Levels") },
            { ProfileTemplate, new("Profile Template") },
            { Required, new("Required") },
            { Skills, new("Skills") },
            {
                StringValidation,
                new("Invalid string {{Name}}, Min {{Min}}, Max {{Max}}, Regexes {{Regexes}}")
            },
            { Afghanistan, new("Afghanistan") },
            { Albania, new("Albania") },
            { Algeria, new("Algeria") },
            { Andorra, new("Andorra") },
            { Angola, new("Angola") },
            { AntiguaAndBarbuda, new("Antigua and Barbuda") },
            { Argentina, new("Argentina") },
            { Armenia, new("Armenia") },
            { Australia, new("Australia") },
            { Austria, new("Austria") },
            { Azerbaijan, new("Azerbaijan") },
            { Bahamas, new("Bahamas") },
            { Bahrain, new("Bahrain") },
            { Bangladesh, new("Bangladesh") },
            { Barbados, new("Barbados") },
            { Belarus, new("Belarus") },
            { Belgium, new("Belgium") },
            { Belize, new("Belize") },
            { Benin, new("Benin") },
            { Bhutan, new("Bhutan") },
            { Bolivia, new("Bolivia") },
            { BosniaAndHerzegovina, new("Bosnia and Herzegovina") },
            { Botswana, new("Botswana") },
            { Brazil, new("Brazil") },
            { Brunei, new("Brunei") },
            { Bulgaria, new("Bulgaria") },
            { BurkinaFaso, new("Burkina Faso") },
            { Burundi, new("Burundi") },
            { CaboVerde, new("Cabo Verde") },
            { Cambodia, new("Cambodia") },
            { Cameroon, new("Cameroon") },
            { Canada, new("Canada") },
            { CentralAfricanRepublic, new("Central African Republic") },
            { Chad, new("Chad") },
            { Chile, new("Chile") },
            { China, new("China") },
            { Colombia, new("Colombia") },
            { Comoros, new("Comoros") },
            { Congo, new("Congo") },
            { CostaRica, new("Costa Rica") },
            { Croatia, new("Croatia") },
            { Cuba, new("Cuba") },
            { Cyprus, new("Cyprus") },
            { CzechRepublicCzechia, new("Czech Republic (Czechia)") },
            { CoteDIvoire, new("Côte d'Ivoire") },
            { Denmark, new("Denmark") },
            { Djibouti, new("Djibouti") },
            { Dominica, new("Dominica") },
            { DominicanRepublic, new("Dominican Republic") },
            { DrCongo, new("DR Congo") },
            { Ecuador, new("Ecuador") },
            { Egypt, new("Egypt") },
            { ElSalvador, new("El Salvador") },
            { EquatorialGuinea, new("Equatorial Guinea") },
            { Eritrea, new("Eritrea") },
            { Estonia, new("Estonia") },
            { Eswatini, new("Eswatini") },
            { Ethiopia, new("Ethiopia") },
            { Fiji, new("Fiji") },
            { Finland, new("Finland") },
            { France, new("France") },
            { Gabon, new("Gabon") },
            { Gambia, new("Gambia") },
            { Georgia, new("Georgia") },
            { Germany, new("Germany") },
            { Ghana, new("Ghana") },
            { Greece, new("Greece") },
            { Grenada, new("Grenada") },
            { Guatemala, new("Guatemala") },
            { Guinea, new("Guinea") },
            { GuineaBissau, new("Guinea-Bissau") },
            { Guyana, new("Guyana") },
            { Haiti, new("Haiti") },
            { HolySee, new("Holy See") },
            { Honduras, new("Honduras") },
            { Hungary, new("Hungary") },
            { Iceland, new("Iceland") },
            { India, new("India") },
            { Indonesia, new("Indonesia") },
            { Iran, new("Iran") },
            { Iraq, new("Iraq") },
            { Ireland, new("Ireland") },
            { Israel, new("Israel") },
            { Italy, new("Italy") },
            { Jamaica, new("Jamaica") },
            { Japan, new("Japan") },
            { Jordan, new("Jordan") },
            { Kazakhstan, new("Kazakhstan") },
            { Kenya, new("Kenya") },
            { Kiribati, new("Kiribati") },
            { Kuwait, new("Kuwait") },
            { Kyrgyzstan, new("Kyrgyzstan") },
            { Laos, new("Laos") },
            { Latvia, new("Latvia") },
            { Lebanon, new("Lebanon") },
            { Lesotho, new("Lesotho") },
            { Liberia, new("Liberia") },
            { Libya, new("Libya") },
            { Liechtenstein, new("Liechtenstein") },
            { Lithuania, new("Lithuania") },
            { Luxembourg, new("Luxembourg") },
            { Madagascar, new("Madagascar") },
            { Malawi, new("Malawi") },
            { Malaysia, new("Malaysia") },
            { Maldives, new("Maldives") },
            { Mali, new("Mali") },
            { Malta, new("Malta") },
            { MarshallIslands, new("Marshall Islands") },
            { Mauritania, new("Mauritania") },
            { Mauritius, new("Mauritius") },
            { Mexico, new("Mexico") },
            { Micronesia, new("Micronesia") },
            { Moldova, new("Moldova") },
            { Monaco, new("Monaco") },
            { Mongolia, new("Mongolia") },
            { Montenegro, new("Montenegro") },
            { Morocco, new("Morocco") },
            { Mozambique, new("Mozambique") },
            { Myanmar, new("Myanmar") },
            { Namibia, new("Namibia") },
            { Nauru, new("Nauru") },
            { Nepal, new("Nepal") },
            { Netherlands, new("Netherlands") },
            { NewZealand, new("New Zealand") },
            { Nicaragua, new("Nicaragua") },
            { Niger, new("Niger") },
            { Nigeria, new("Nigeria") },
            { NorthKorea, new("North Korea") },
            { NorthMacedonia, new("North Macedonia") },
            { Norway, new("Norway") },
            { Oman, new("Oman") },
            { Pakistan, new("Pakistan") },
            { Palau, new("Palau") },
            { Panama, new("Panama") },
            { PapuaNewGuinea, new("Papua New Guinea") },
            { Paraguay, new("Paraguay") },
            { Peru, new("Peru") },
            { Philippines, new("Philippines") },
            { Poland, new("Poland") },
            { Portugal, new("Portugal") },
            { Qatar, new("Qatar") },
            { Romania, new("Romania") },
            { Russia, new("Russia") },
            { Rwanda, new("Rwanda") },
            { SaintKittsAndNevis, new("Saint Kitts & Nevis") },
            { SaintLucia, new("Saint Lucia") },
            { Samoa, new("Samoa") },
            { SanMarino, new("San Marino") },
            { SaoTomeAndPrincipe, new("Sao Tome & Principe") },
            { SaudiArabia, new("Saudi Arabia") },
            { Senegal, new("Senegal") },
            { Serbia, new("Serbia") },
            { Seychelles, new("Seychelles") },
            { SierraLeone, new("Sierra Leone") },
            { Singapore, new("Singapore") },
            { Slovakia, new("Slovakia") },
            { Slovenia, new("Slovenia") },
            { SolomonIslands, new("Solomon Islands") },
            { Somalia, new("Somalia") },
            { SouthAfrica, new("South Africa") },
            { SouthKorea, new("South Korea") },
            { SouthSudan, new("South Sudan") },
            { Spain, new("Spain") },
            { SriLanka, new("Sri Lanka") },
            { StVincentAndGrenadines, new("St. Vincent & Grenadines") },
            { StateOfPalestine, new("State of Palestine") },
            { Sudan, new("Sudan") },
            { Suriname, new("Suriname") },
            { Sweden, new("Sweden") },
            { Switzerland, new("Switzerland") },
            { Syria, new("Syria") },
            { Tajikistan, new("Tajikistan") },
            { Tanzania, new("Tanzania") },
            { Thailand, new("Thailand") },
            { TimorLeste, new("Timor-Leste") },
            { Togo, new("Togo") },
            { Tonga, new("Tonga") },
            { TrinidadAndTobago, new("Trinidad and Tobago") },
            { Tunisia, new("Tunisia") },
            { Turkey, new("Turkey") },
            { Turkmenistan, new("Turkmenistan") },
            { Tuvalu, new("Tuvalu") },
            { Uganda, new("Uganda") },
            { Ukraine, new("Ukraine") },
            { UnitedArabEmirates, new("United Arab Emirates") },
            { UnitedKingdom, new("United Kingdom") },
            { UnitedStates, new("United States") },
            { Uruguay, new("Uruguay") },
            { Uzbekistan, new("Uzbekistan") },
            { Vanuatu, new("Vanuatu") },
            { Venezuela, new("Venezuela") },
            { Vietnam, new("Vietnam") },
            { Yemen, new("Yemen") },
            { Zambia, new("Zambia") },
            { Zimbabwe, new("Zimbabwe") },
        };
}
