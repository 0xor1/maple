﻿using Common;
using Dnsk.Db;
using Dnsk.Proto;
using Dnsk.Service.Util;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Dnsk.I18n;

namespace Dnsk.Service.Services;

public class ApiService : Api.ApiBase
{
    private readonly DnskDb _db;
    private readonly IEmailClient _emailClient;
    
    public ApiService(DnskDb db, IEmailClient emailClient)
    {
        _db = db;
        _emailClient = emailClient;
    }

    public override Task<Auth_Session> Auth_GetSession(Nothing _, ServerCallContext stx)
    {
        var ses = stx.GetSession();
        var a_ses = new Auth_Session()
        {
            Id = ses.Id,
            IsAuthed = ses.IsAuthed,
            Lang = ses.Lang,
            DateFmt = ses.DateFmt,
            TimeFmt = ses.TimeFmt
        };
        return a_ses.Task();
    }

    public override async Task<Nothing> Auth_Register(Auth_RegisterReq req, ServerCallContext stx)
    {
        // basic validation
        var ses = stx.GetSession();
        stx.ErrorIf(ses.IsAuthed, Strings.AlreadyAuthenticated);
        // !!! ToLower all emails in all Auth_ api endpoints
        req.Email = req.Email.ToLower();
        stx.ErrorFromValidationResult(AuthValidator.Email(req.Email));
        stx.ErrorFromValidationResult(AuthValidator.Pwd(req.Pwd));
        
        // start db tx
        await using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            var existing = await _db.Auths.SingleOrDefaultAsync(x => x.Email.Equals(req.Email) || x.NewEmail.Equals(req.Email));
            var newCreated = existing == null;
            if (existing == null)
            {
                var verifyEmailCode = Crypto.String(32);
                var pwd = Crypto.HashPwd(req.Pwd);
                existing = new Auth()
                {
                    Id = ses.Id,
                    Email = req.Email,
                    VerifyEmailCodeCreatedOn = DateTime.UtcNow,
                    VerifyEmailCode = verifyEmailCode,
                    PwdVersion = pwd.PwdVersion,
                    PwdSalt = pwd.PwdSalt,
                    PwdHash = pwd.PwdHash,
                    PwdIters = pwd.PwdIters
                };
                await _db.Auths.AddAsync(existing, stx.CancellationToken);
                await _db.SaveChangesAsync();
            }

            if (!existing.VerifyEmailCode.IsNullOrEmpty() && 
                (newCreated || 
                 (existing.VerifyEmailCodeCreatedOn.MinutesSince() > 10 && 
                  existing.ActivatedOn.IsZero())))
            {
                // if there is a verify email code and
                // we've just registered a new account
                // or the verify email was sent over 10 mins ago
                // and the account is not yet activated
                var model = new
                {
                    BaseHref = Config.Server.Listen,
                    Email = existing.Email,
                    Code = existing.VerifyEmailCode
                };
                await _emailClient.SendEmailAsync(
                    stx.String(Strings.AuthConfirmEmailSubject), 
                     stx.String(Strings.AuthConfirmEmailHtml, model), 
                    stx.String(Strings.AuthConfirmEmailText, model), 
                    Config.Email.NoReplyAddress, 
                    new List<string>(){req.Email});
            }
            await tx.CommitAsync();
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }

        return new Nothing();
    }

    public override async Task<Nothing> Auth_VerifyEmail(Auth_VerifyEmailReq req, ServerCallContext stx)
    {
        // !!! ToLower all emails in all Auth_ api endpoints
        req.Email = req.Email.ToLower();
        stx.ErrorFromValidationResult(AuthValidator.Email(req.Email));
        
        // start db tx
        await using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            var auth = await _db.Auths.SingleOrDefaultAsync(x => x.Email.Equals(req.Email) || x.NewEmail.Equals(req.Email));
            stx.ErrorIf(auth == null, Strings.NoMatchingRecord);
            stx.ErrorIf(auth.NotNull().VerifyEmailCode != req.Code, Strings.InvalidEmailCode);
            if (!auth.NewEmail.IsNullOrEmpty() && auth.NewEmail == req.Email)
            {
                // verifying new email
                auth.Email = auth.NewEmail;
                auth.NewEmail = string.Empty;
            }
            else
            {
                // first account activation
                auth.ActivatedOn = DateTime.UtcNow;
            }

            auth.VerifyEmailCodeCreatedOn = DateTimeExts.Zero();
            auth.VerifyEmailCode = string.Empty;
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }

        return new Nothing();
    }
    
    public override async Task<Nothing> Auth_SendResetPwdEmail(Auth_SendResetPwdEmailReq req, ServerCallContext stx)
    {
        // basic validation
        var ses = stx.GetSession(); 
        stx.ErrorIf(ses.IsAuthed, Strings.AlreadyAuthenticated);
        // !!! ToLower all emails in all Auth_ api endpoints
        req.Email = req.Email.ToLower();
        stx.ErrorFromValidationResult(AuthValidator.Email(req.Email));
        
        // start db tx
        await using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            var existing = await _db.Auths.SingleOrDefaultAsync(x => x.Email.Equals(req.Email));
            if (existing == null || existing.ResetPwdCodeCreatedOn.MinutesSince() < 10)
            {
                // if email is not associated with an account or
                // a reset pwd was sent within the last 10 minutes
                // dont do anything
                return new Nothing();
            }

            existing.ResetPwdCodeCreatedOn = DateTime.UtcNow;
            existing.ResetPwdCode = Crypto.String(32);
            await _db.SaveChangesAsync();
            var model = new
            {
                BaseHref = Config.Server.Listen,
                Email = existing.Email,
                Code = existing.ResetPwdCode
            };
            await _emailClient.SendEmailAsync(
                stx.String(Strings.AuthResetPwdSubject),
                stx.String(Strings.AuthResetPwdHtml, model),
                stx.String(Strings.AuthResetPwdText, model),
                Config.Email.NoReplyAddress,
                new List<string>(){req.Email}
            );
            await tx.CommitAsync();
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }

        return new Nothing();
    }

    public override async Task<Nothing> Auth_ResetPwd(Auth_ResetPwdReq req, ServerCallContext stx)
    {
        // !!! ToLower all emails in all Auth_ api endpoints
        req.Email = req.Email.ToLower();
        stx.ErrorFromValidationResult(AuthValidator.Email(req.Email));
        stx.ErrorFromValidationResult(AuthValidator.Pwd(req.NewPwd));
        
        // start db tx
        await using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            var auth = await _db.Auths.SingleOrDefaultAsync(x => x.Email.Equals(req.Email));
            stx.ErrorIf(auth == null, Strings.NoMatchingRecord);
            stx.ErrorIf(auth.NotNull().ResetPwdCode != req.Code, Strings.InvalidResetPwdCode);
            var pwd = Crypto.HashPwd(req.NewPwd);
            auth.ResetPwdCodeCreatedOn = DateTimeExts.Zero();
            auth.ResetPwdCode = string.Empty;
            auth.PwdVersion = pwd.PwdVersion;
            auth.PwdSalt = pwd.PwdSalt;
            auth.PwdHash = pwd.PwdHash;
            auth.PwdIters = pwd.PwdIters;
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }

        return new Nothing();
    }

    public override async Task<Auth_Session> Auth_SignIn(Auth_SignInReq req, ServerCallContext stx)
    {
        // basic validation
        var ses = stx.GetSession();
        stx.ErrorIf(ses.IsAuthed, Strings.AlreadyAuthenticated);
        // !!! ToLower all emails in all Auth_ api endpoints
        req.Email = req.Email.ToLower();
        stx.ErrorFromValidationResult(AuthValidator.Email(req.Email));
        
        // start db tx
        await using var tx = await _db.Database.BeginTransactionAsync();
        var auth = await _db.Auths.SingleOrDefaultAsync(x => x.Email.Equals(req.Email));
        stx.ErrorIf(auth == null, Strings.NoMatchingRecord);
        stx.ErrorIf(auth.NotNull().ActivatedOn.IsZero(), Strings.AccountNotVerified);
        RateLimitAuthAttempts(stx, auth.NotNull());
        auth.LastSignInAttemptOn = DateTime.UtcNow;
        var pwdIsValid = Crypto.PwdIsValid(req.Pwd, auth);
        if (pwdIsValid)
        {
            auth.LastSignedInOn = DateTime.UtcNow;
            ses = stx.SignIn(auth.Id, req.RememberMe);
        }
        await _db.SaveChangesAsync();
        await tx.CommitAsync();
        stx.ErrorIf(!pwdIsValid, Strings.NoMatchingRecord);
        return new Auth_Session()
        {
            Id = ses.Id,
            IsAuthed = ses.IsAuthed
        };
    }

    public override Task<Auth_Session> Auth_SignOut(Nothing _, ServerCallContext stx)
    {
        // basic validation
        var ses = stx.GetSession();
        if (ses.IsAuthed)
        {
            ses = stx.SignOut();
        }
        return new Auth_Session()
        {
            Id = ses.Id,
            IsAuthed = ses.IsAuthed
        }.Task();
    }

    // public override async Task<Auth_Session> Auth_SetL10n(Auth_SetL10nReq req, ServerCallContext stx)
    // {
    //     var ses = _session.Get(stx);
    //     TODO update session values and IF it's an authed session update the DB values 
    // }
    
    private const int AuthAttemptsRateLimit = 5;
    private static void RateLimitAuthAttempts(ServerCallContext stx, Auth auth)
    {
        stx.ErrorIf(auth.LastSignInAttemptOn.SecondsSince() < AuthAttemptsRateLimit, Strings.AuthAttemptRateLimit);
    }
}