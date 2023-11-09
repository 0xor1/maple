﻿using Common.Shared;

namespace Dnsk.I18n;

public static partial class S
{
    private static readonly IReadOnlyDictionary<string, TemplatableString> English = new Dictionary<
        string,
        TemplatableString
    >()
    {
        { Demo, new("Demo") },
        { DemoTitle, new("This app is for demonstration purposes only.") },
        {
            Dnsk,
            new(
                "<h1>Hello, Dnsk!</h1><p>Welcome to your new dotnet starter kit.</p><p>You will find:</p><ul><li>Client: a blazor wasm app using radzen ui library</li><li>Server: aspnet with rpc pattern api and entity framework db interface</li></ul>"
            )
        },
        { Invalid, new("Invalid") },
        { RpcUnknownEndpoint, new("Unknown RPC endpoint") },
        { UnexpectedError, new("An unexpected error occurred") },
        { EntityNotFound, new("{{Name}} not found") },
        { InsufficientPermission, new("Insufficient permission") },
        { ApiError, new("Api Error") },
        { MinMaxNullArgs, new("Both min and  max arguments are null") },
        { MinMaxReversedArgs, new("Min {{Min}} and Max {{Max}} values are out of ordered") },
        { BadRequest, new("Bad request") },
        { RequestBodyTooLarge, new("Request body too large, limit {{MaxSize}} bytes") },
        { AuthInvalidEmail, new("Invalid email") },
        { AuthInvalidPwd, new("Invalid password") },
        { LessThan8Chars, new("Less than 8 characters") },
        { NoLowerCaseChar, new("No lowercase character") },
        { NoUpperCaseChar, new("No uppercase character") },
        { NoDigit, new("No digit") },
        { NoSpecialChar, new("No special character") },
        { AuthAlreadyAuthenticated, new("Already in authenticated session") },
        { AuthNotAuthenticated, new("Not in authenticated session") },
        { AuthInvalidEmailCode, new("Invalid email code") },
        { AuthInvalidResetPwdCode, new("Invalid reset password code") },
        {
            AuthAccountNotVerified,
            new("Account not verified, please check your emails for verification link")
        },
        {
            AuthAttemptRateLimit,
            new(
                "Authentication attempts cannot be made more frequently than every {{Seconds}} seconds"
            )
        },
        { AuthConfirmEmailSubject, new("Confirm Email Address") },
        {
            AuthConfirmEmailHtml,
            new(
                "<div><a href=\"{{BaseHref}}/verify_email?email={{Email}}&code={{Code}}\">Please click this link to verify your email address</a></div >"
            )
        },
        {
            AuthConfirmEmailText,
            new(
                "Please use this link to verify your email address: {{BaseHref}}/verify_email?email={{Email}}&code={{Code}}"
            )
        },
        { AuthResetPwdSubject, new("Reset Password") },
        {
            AuthResetPwdHtml,
            new(
                "<div><a href=\"{{BaseHref}}/reset_pwd?email={{Email}}&code={{Code}}\">Please click this link to reset your password</a></div>"
            )
        },
        {
            AuthResetPwdText,
            new(
                "Please click this link to reset your password: {{BaseHref}}/reset_pwd?email={{Email}}&code={{Code}}"
            )
        },
        { AuthFcmTopicInvalid, new("Fcm topic invalid Min: {{Min}}, Max: {{Max}}") },
        { AuthFcmTokenInvalid, new("Fcm token invalid") },
        { AuthFcmNotEnabled, new("Fcm not enabled") },
        { AuthFcmMessageInvalid, new("Fcm message invalid") },
        { Home, new("Home") },
        { L10n, new("Localization") },
        { ToggleLiveUpdates, new("Toggle live updates") },
        { Live, new("Live:") },
        { On, new("On") },
        { Off, new("Off") },
        { Language, new("Language") },
        { DateFmt, new("Date Format") },
        { TimeFmt, new("Time Format") },
        { Register, new("Register") },
        { Registering, new("Registering") },
        {
            RegisterSuccess,
            new("Please check your emails for a confirmation link to complete registration.")
        },
        { SignIn, new("Sign In") },
        { RememberMe, new("Remember Me") },
        { SigningIn, new("Signing In") },
        { SignOut, new("Sign Out") },
        { SigningOut, new("Signing Out") },
        { VerifyEmail, new("Verify Email") },
        { Verifying, new("Verifying") },
        { VerifyingEmail, new("Verifying your email") },
        { VerifyEmailSuccess, new("Thank you for verifying your email.") },
        { ResetPwd, new("Reset Password") },
        { Email, new("Email") },
        { Pwd, new("Password") },
        { ConfirmPwd, new("Confirm Password") },
        { PwdsDontMatch, new("Passwords don't match") },
        { ResetPwdSuccess, new("You can now sign in with your new password.") },
        { Resetting, new("Resetting") },
        { SendResetPwdLink, new("Send Reset Password Link") },
        {
            SendResetPwdLinkSuccess,
            new(
                "If this email matches an account an email will have been sent to reset your password."
            )
        },
        { Processing, new("Processing") },
        { Send, new("Send") },
        { Success, new("Success") },
        { Error, new("Error") },
        { Update, new("Update") },
        { Updating, new("Updating") },
        { Counter, new("Counter") },
        { Increment, new("Increment") },
        { Decrement, new("Decrement") },
        { Incrementing, new("Incrementing") },
        { Decrementing, new("Decrementing") }
    };
}
