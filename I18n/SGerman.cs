﻿using Fluid;

namespace Dnsk.I18n;

public static partial class S
{
    private static readonly IReadOnlyDictionary<string, IFluidTemplate> German = new Dictionary<string, IFluidTemplate>()
    {
        { Dnsk, Parser.Parse("<h1>Hallo, Dnsk!</h1><p>Willkommen bei Ihrem neuen dotnet-Starterkit.</p><p>Sie finden:</p><ul><li>Client: eine Blazor-Wasm-App, die verwendet radzen ui library</li><li>Server: aspnet mit grpc api und Entity Framework db interface</li></ul>")},
        { Invalid, Parser.Parse("Ungültig") },
        { InvalidEmail, Parser.Parse("Ungültige E-Mail") },
        { InvalidPwd, Parser.Parse("Ungültiges Passwort") },
        { LessThan8Chars, Parser.Parse("Weniger als 8 Zeichen") },
        { NoLowerCaseChar, Parser.Parse("Kein Kleinbuchstabe") },
        { NoUpperCaseChar, Parser.Parse("Kein Großbuchstabe") },
        { NoDigit, Parser.Parse("Keine Ziffer") },
        { NoSpecialChar, Parser.Parse("Kein Sonderzeichen") },
        { UnexpectedError, Parser.Parse("Ein unerwarteter Fehler ist aufgetreten") },
        { AlreadyAuthenticated, Parser.Parse("Bereits in authentifizierter Sitzung") },
        { NoMatchingRecord, Parser.Parse("Kein übereinstimmender Datensatz") },
        { InvalidEmailCode, Parser.Parse("Ungültiger E-Mail-Code") },
        { InvalidResetPwdCode, Parser.Parse("Ungültiger Passwort-Reset-Code") },
        { AccountNotVerified, Parser.Parse("Konto nicht bestätigt, bitte überprüfen Sie Ihre E-Mails auf den Bestätigungslink") },
        { AuthAttemptRateLimit, Parser.Parse("Authentifizierungsversuche können nicht häufiger als alle {{Seconds}} Sekunden durchgeführt werden") },
        { AuthConfirmEmailSubject, Parser.Parse("e-Mail-Adresse bestätigen")}, 
        { AuthConfirmEmailHtml, Parser.Parse("<div><a href=\"{{BaseHref}}/verify_email?email={{Email}}&code={{Code}}\">Bitte klicken Sie auf diesen Link, um Ihre E-Mail-Adresse zu bestätigen</a></div>")}, 
        { AuthConfirmEmailText, Parser.Parse("Bitte verwenden Sie diesen Link, um Ihre E-Mail-Adresse zu bestätigen: {{BaseHref}}/verify_email?email={{Email}}&code={{Code}}")},
        { AuthResetPwdSubject, Parser.Parse("Passwort zurücksetzen")}, 
        { AuthResetPwdHtml, Parser.Parse("<div><a href=\"{{BaseHref}}/reset_pwd?email={{Email}}&code={{Code}}\">klicken Sie bitte auf diesen Link, um Ihr Passwort zurückzusetzen</a></div>")}, 
        { AuthResetPwdText, Parser.Parse("Bitte klicken Sie auf diesen Link, um Ihr Passwort zurückzusetzen: {{BaseHref}}/reset_pwd?email={{Email}}&code={{Code}}")},
        { Home, Parser.Parse("Heim")},
        { L10n, Parser.Parse("Lokalisierung")},
        { Language, Parser.Parse("Sprache")},
        { DateFmt, Parser.Parse("Datumsformat")},
        { TimeFmt, Parser.Parse("Zeitformat")},
        { Register, Parser.Parse("Registrieren")},
        { Registering, Parser.Parse("Registrieren")},
        { RegisterSuccess, Parser.Parse("Bitte überprüfen Sie Ihre E-Mails auf einen Bestätigungslink, um die Registrierung abzuschließen.")},
        { SignIn, Parser.Parse("Anmelden")},
        { RememberMe, Parser.Parse("Mich erinnern")},
        { SigningIn, Parser.Parse("Anmelden")},
        { SignOut, Parser.Parse("Austragen")},
        { SigningOut, Parser.Parse("Abmelden")},
        { VerifyEmail, Parser.Parse("E-Mail bestätigen")},
        { Verifying, Parser.Parse("Überprüfung")},
        { VerifyingEmail, Parser.Parse("Überprüfung Ihrer E-Mail")},
        { VerifyEmailSuccess, Parser.Parse("Danke für das Verifizieren deiner E-Mail.")},
        { ResetPwd, Parser.Parse("Passwort zurücksetzen")},
        { Email, Parser.Parse("Email")},
        { Pwd, Parser.Parse("Passwort")},
        { ConfirmPwd, Parser.Parse("Bestätige das Passwort")},
        { PwdsDontMatch, Parser.Parse("Passwörter stimmen nicht überein")},
        { ResetPwdSuccess, Parser.Parse("Sie können sich jetzt mit Ihrem neuen Passwort anmelden.")},
        { Resetting, Parser.Parse("Zurücksetzen")},
        { SendResetPwdLink, Parser.Parse("Link zum Zurücksetzen des Passworts senden")},
        { SendResetPwdLinkSuccess, Parser.Parse("Wenn diese E-Mail mit einem Konto übereinstimmt, wurde eine E-Mail zum Zurücksetzen Ihres Passworts gesendet.")},
        { Processing, Parser.Parse("wird bearbeitet")},
        { Send, Parser.Parse("Schicken")},
        { Success, Parser.Parse("Erfolg")},
        { Error, Parser.Parse("Fehler")}
    };
}