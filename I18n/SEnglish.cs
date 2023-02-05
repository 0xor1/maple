﻿using Fluid;

namespace Dnsk.I18n;

public static partial class S
{
    private static readonly IReadOnlyDictionary<string, IFluidTemplate> English = new Dictionary<string, IFluidTemplate>()
    {
        { Dnsk, Parser.Parse("<h1>Hello, Dnsk!</h1><p>Welcome to your new dotnet starter kit.</p><p>You will find:</p><ul><li>Client: a blazor wasm app using radzen ui library</li><li>Server: aspnet with grpc api and entity framework db interface</li></ul>")},
        { Invalid, Parser.Parse("Invalid") },
        { InvalidEmail, Parser.Parse("Invalid email") },
        { InvalidPwd, Parser.Parse("Invalid password") },
        { LessThan8Chars, Parser.Parse("Less than 8 characters") },
        { NoLowerCaseChar, Parser.Parse("No lowercase character") },
        { NoUpperCaseChar, Parser.Parse("No uppercase character") },
        { NoDigit, Parser.Parse("No digit") },
        { NoSpecialChar, Parser.Parse("No special character") },
        { UnexpectedError, Parser.Parse("An unexpected error occurred") },
        { AlreadyAuthenticated, Parser.Parse("Already in authenticated session") },
        { NoMatchingRecord, Parser.Parse("No matching record") },
        { InvalidEmailCode, Parser.Parse("Invalid email code") },
        { InvalidResetPwdCode, Parser.Parse("Invalid reset password code") },
        { AccountNotVerified, Parser.Parse("Account not verified, please check your emails for verification link") },
        { AuthAttemptRateLimit, Parser.Parse("Authentication attempts cannot be made more frequently than every {{Seconds}} seconds") },
        { AuthConfirmEmailSubject, Parser.Parse("Confirm Email Address")}, 
        { AuthConfirmEmailHtml, Parser.Parse("<div><a href=\"{{BaseHref}}/verify_email?email={{Email}}&code={{Code}}\">Please click this link to verify your email address</a></div >")}, 
        { AuthConfirmEmailText, Parser.Parse("Please use this link to verify your email address: {{BaseHref}}/verify_email?email={{Email}}&code={{Code}}")},
        { AuthResetPwdSubject, Parser.Parse("Reset Password")}, 
        { AuthResetPwdHtml, Parser.Parse("<div><a href=\"{{BaseHref}}/reset_pwd?email={{Email}}&code={{Code}}\">Please click this link to reset your password</a></div>")}, 
        { AuthResetPwdText, Parser.Parse("Please click this link to reset your password: {{BaseHref}}/reset_pwd?email={{Email}}&code={{Code}}")},
        { Home, Parser.Parse("Home")},
        { L10n, Parser.Parse("Localization")},
        { Language, Parser.Parse("Language")},
        { DateFmt, Parser.Parse("Date Format")},
        { TimeFmt, Parser.Parse("Time Format")},
        { Register, Parser.Parse("Register")},
        { Registering, Parser.Parse("Registering")},
        { RegisterSuccess, Parser.Parse("Please check your emails for a confirmation link to complete registration.")},
        { SignIn, Parser.Parse("Sign In")},
        { SigningIn, Parser.Parse("Signing In")},
        { SignOut, Parser.Parse("Sign Out")},
        { SigningOut, Parser.Parse("Signing Out")},
        { VerifyEmail, Parser.Parse("Verify Email")},
        { Verifying, Parser.Parse("Verifying")},
        { VerifyingEmail, Parser.Parse("Verifying your email")},
        { VerifyEmailSuccess, Parser.Parse("Thank you for verifying your email.")},
        { ResetPwd, Parser.Parse("Reset Password")},
        { Email, Parser.Parse("Email")},
        { Pwd, Parser.Parse("Password")},
        { ConfirmPwd, Parser.Parse("Confirm Password")},
        { PwdsDontMatch, Parser.Parse("Passwords don't match")},
        { ResetPwdSuccess, Parser.Parse("You can now sign in with your new password.")},
        { Resetting, Parser.Parse("Resetting")},
        { SendResetPwdLink, Parser.Parse("Send Reset Password Link")},
        { SendResetPwdLinkSuccess, Parser.Parse("If this email matches an account an email will have been sent to reset your password.")},
        { Processing, Parser.Parse("Processing")},
        { Send, Parser.Parse("Send")},
        { Success, Parser.Parse("Success")},
        { Error, Parser.Parse("Error")}
    };
}