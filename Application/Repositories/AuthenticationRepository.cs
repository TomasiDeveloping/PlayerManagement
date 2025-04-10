using System.IdentityModel.Tokens.Jwt;
using Application.Classes;
using Application.DataTransferObjects.Authentication;
using Application.Errors;
using Application.Helpers;
using Application.Helpers.Email;
using Application.Interfaces;
using AutoMapper;
using Database;
using Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Utilities.Classes;
using Utilities.Constants;
using Utilities.Interfaces;

namespace Application.Repositories;

public class AuthenticationRepository(UserManager<User> userManager, ApplicationContext context, IMapper mapper, IJwtService jwtService, ILogger<AuthenticationRepository> logger, IEmailService emailService) : IAuthenticationRepository
{
    public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken)
    {
        var userToLogin = await userManager.FindByEmailAsync(loginRequestDto.Email);

        if (userToLogin is null || !await userManager.CheckPasswordAsync(userToLogin, loginRequestDto.Password))
        {
            return Result.Failure<LoginResponseDto>(AuthenticationErrors.LoginFailed);
        }

        if (!await userManager.IsEmailConfirmedAsync(userToLogin))
        {
            return Result.Failure<LoginResponseDto>(AuthenticationErrors.EmailNotConfirmed);
        }

        var loginResponse = new LoginResponseDto()
        {
            Token = await CreateJwtToken(userToLogin)
        };

        return Result.Success(loginResponse);
    }

    public async Task<Result> RegisterUserAsync(RegisterUserDto registerUserDto, CancellationToken cancellationToken)
    {
        var allianceForUser = await context.Alliances
            .AsNoTracking()
            .FirstOrDefaultAsync(alliance => alliance.Id == registerUserDto.AllianceId, cancellationToken);

        if (allianceForUser is null) return Result.Failure(AllianceErrors.NotFound);

        var userRole = await context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(role => role.Id == registerUserDto.RoleId, cancellationToken);

        if (userRole is null) return Result.Failure(RoleErrors.NotFound);

        var newUser = mapper.Map<User>(registerUserDto);
        newUser.AllianceId = allianceForUser.Id;
        newUser.EmailConfirmed = false;

        var userCreateResult = await userManager.CreateAsync(newUser, registerUserDto.Password);

        if (!userCreateResult.Succeeded) return Result.Failure(AuthenticationErrors.RegisterFailed);

        var addRoleResult = await userManager.AddToRoleAsync(newUser, userRole.Name!);

        if (!addRoleResult.Succeeded)
        {
            await userManager.DeleteAsync(newUser);
            return Result.Failure(AuthenticationErrors.RegisterFailed);
        }

        var emailTemplate = EmailTemplateFactory.GetEmailTemplate("en");

        var emailConfirmToken = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
        var callBack = new Uri(registerUserDto.EmailConfirmUri)
            .AddQueryParam("token", emailConfirmToken)
            .AddQueryParam("email", registerUserDto.Email);

        var confirmEmailContent = emailTemplate.ConfirmEmail(newUser.PlayerName, callBack.ToString());

        var emailConfirmMessage = new EmailMessage([registerUserDto.Email], confirmEmailContent.Subject,
            confirmEmailContent.Content);

        var emailSendResponse = await emailService.SendEmailAsync(emailConfirmMessage);

        if (emailSendResponse) return Result.Success();

        await userManager.DeleteAsync(newUser);
        return Result.Failure(AuthenticationErrors.RegisterFailed);
    }

    public async Task<Result> EmailConfirmationAsync(ConfirmEmailRequestDto confirmEmailRequestDto)
    {
        var userToConfirm = await userManager.FindByEmailAsync(confirmEmailRequestDto.Email);

        if (userToConfirm is null) return Result.Failure(UserErrors.NotFound);

        var confirmEmailResult = await userManager.ConfirmEmailAsync(userToConfirm, confirmEmailRequestDto.Token);

        return !confirmEmailResult.Succeeded ? Result.Failure(AuthenticationErrors.EmailNotConfirmed) : Result.Success();
    }

    public async Task<Result> ResendConfirmationEmailAsync(EmailConfirmationRequestDto emailConfirmationRequestDto)
    {
        var user = await userManager.FindByEmailAsync(emailConfirmationRequestDto.Email);

        if (user is null) return Result.Failure(UserErrors.NotFound);

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

        var callback = new Uri(emailConfirmationRequestDto.ClientUri)
            .AddQueryParam("token", token)
            .AddQueryParam("email", emailConfirmationRequestDto.Email);

        var emailTemplate = EmailTemplateFactory.GetEmailTemplate("en");

        var emailContent = emailTemplate.ResendConfirmationEmail(user.PlayerName, callback.ToString());

        var emailMessage =
            new EmailMessage([emailConfirmationRequestDto.Email], emailContent.Subject, emailContent.Content);

        var sendMailResponse = await emailService.SendEmailAsync(emailMessage);

        return sendMailResponse
            ? Result.Success()
            : Result.Failure(AuthenticationErrors.ResendConfirmationEmailFailed);
    }

    public async Task<Result> InviteUserAsync(InviteUserDto inviteUserDto, CancellationToken cancellationToken)
    {
        var invitingUser = await userManager.FindByIdAsync(inviteUserDto.InvitingUserId.ToString());

        if (invitingUser is null) return Result.Failure(UserErrors.NotFound);

        var allianceForUser = await context.Alliances
            .AsNoTracking()
            .FirstOrDefaultAsync(alliance => alliance.Id == invitingUser.AllianceId, cancellationToken);

        if (allianceForUser is null) return Result.Failure(AllianceErrors.NotFound);

        var roleForUser = await context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(role => role.Name == inviteUserDto.Role, cancellationToken);

        if (roleForUser is null) return Result.Failure(RoleErrors.NotFound);

        var emailTemplate = EmailTemplateFactory.GetEmailTemplate("en");

        var callBack = new Uri(inviteUserDto.RegisterUserUri)
            .AddQueryParam("email", inviteUserDto.Email)
            .AddQueryParam("allianceId", inviteUserDto.AllianceId.ToString())
            .AddQueryParam("role", roleForUser.Id.ToString());

        var inviteUserEmailContent =
            emailTemplate.InviteUserEmail(invitingUser.PlayerName, allianceForUser.Name, callBack.ToString());

        var inviteUserEmailMessage = new EmailMessage([inviteUserDto.Email], inviteUserEmailContent.Subject,
            inviteUserEmailContent.Content);

        var emailSendResponse = await emailService.SendEmailAsync(inviteUserEmailMessage);

        return emailSendResponse ? Result.Success() : Result.Failure(AuthenticationErrors.InviteUserFailed);
    }

    public async Task<Result> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        var user = await userManager.FindByEmailAsync(resetPasswordDto.Email);

        if (user is null) return Result.Failure(UserErrors.NotFound);

        var resetPasswordResult =
            await userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);

        return resetPasswordResult.Succeeded
            ? Result.Success()
            : Result.Failure(new Error("Error.ResetPassword.Failed", "Reset password failed"));
    }

    public async Task<Result> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        var user = await userManager.FindByEmailAsync(forgotPasswordDto.Email);

        if (user is null) return Result.Failure(UserErrors.NotFound);

        var resetPasswordToken = await userManager.GeneratePasswordResetTokenAsync(user);

        var emailTemplate = EmailTemplateFactory.GetEmailTemplate("en");

        var callback = new Uri(forgotPasswordDto.ResetPasswordUri)
            .AddQueryParam("token", resetPasswordToken)
            .AddQueryParam("email", forgotPasswordDto.Email);

        var resetPasswordContent = emailTemplate.ResetPasswordEmail(user.PlayerName, callback.ToString());

        var resetPasswordEmailMessage = new EmailMessage([forgotPasswordDto.Email], resetPasswordContent.Subject,
            resetPasswordContent.Content);

        var emailSendResponse = await emailService.SendEmailAsync(resetPasswordEmailMessage);

        return emailSendResponse ? Result.Success() : Result.Failure(new Error("Error.ResetPassword.SendMail", "Could not send the email"));
    }

    public async Task<Result> RegisterToApplicationAsync(SignUpRequestDto signUpRequestDto, CancellationToken cancellationToken)
    {
        var newUser = mapper.Map<User>(signUpRequestDto);
        var userAlliance = mapper.Map<Alliance>(signUpRequestDto);

        var checkAllianceExists = await AllianceAlreadyExists(userAlliance.Server, userAlliance.Abbreviation, cancellationToken);

        if (checkAllianceExists) return Result.Failure(AuthenticationErrors.AllianceAlreadyExists);

        var createAllianceResult = await CreateAlliance(userAlliance, cancellationToken);

        if (createAllianceResult.IsFailure) return Result.Failure(createAllianceResult.Error);

        newUser.AllianceId = createAllianceResult.Value.Id;
        newUser.EmailConfirmed = false;

        var userCreateResult = await userManager.CreateAsync(newUser, signUpRequestDto.Password);

        if (!userCreateResult.Succeeded)
        {
            var rollBackResult = await RollbackAlliance(createAllianceResult.Value, cancellationToken);

            return Result.Failure(rollBackResult.IsFailure ? rollBackResult.Error : AuthenticationErrors.RegisterFailed);
        }

        var addRoleResult = await userManager.AddToRoleAsync(newUser, ApplicationRoles.Administrator);

        if (!addRoleResult.Succeeded)
        {
            await userManager.DeleteAsync(newUser);
            await RollbackAlliance(createAllianceResult.Value, cancellationToken);
            return Result.Failure(AuthenticationErrors.RegisterFailed);
        }

        var emailTemplate = EmailTemplateFactory.GetEmailTemplate("en");

        var emailConfirmToken = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
        var callBack = new Uri(signUpRequestDto.EmailConfirmUri)
            .AddQueryParam("token", emailConfirmToken)
            .AddQueryParam("email", signUpRequestDto.Email);

        var confirmEmailContent = emailTemplate.ConfirmEmail(newUser.PlayerName, callBack.ToString());

        var emailConfirmMessage = new EmailMessage([signUpRequestDto.Email], confirmEmailContent.Subject,
            confirmEmailContent.Content);

        var emailSendResponse = await emailService.SendEmailAsync(emailConfirmMessage);

        if (emailSendResponse)
        {
            logger.LogInformation("User {UserName} with Alliance: {AllianzName} registered successfully", newUser.PlayerName, createAllianceResult.Value.Name);
            return Result.Success();
        }

        await userManager.DeleteAsync(newUser);
        await RollbackAlliance(createAllianceResult.Value, cancellationToken);
        return Result.Failure(AuthenticationErrors.RegisterFailed);
    }

    private async Task<Result<Alliance>> CreateAlliance(Alliance alliance, CancellationToken cancellationToken)
    {
        await context.Alliances.AddAsync(alliance, cancellationToken);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return alliance;
        }
        catch (Exception e)
        {
            logger.LogError(e, "{DateBaseErrorMessage}", e.Message);
            return Result.Failure<Alliance>(GeneralErrors.DatabaseError);
        }
    }

    private async Task<Result<bool>> RollbackAlliance(Alliance alliance, CancellationToken cancellationToken)
    {
        context.Alliances.Remove(alliance);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(true);
        }
        catch (Exception e)
        {
            logger.LogError(e, "{DateBaseErrorMessage}", e.Message);
            return Result.Failure<bool>(GeneralErrors.DatabaseError);
        }
    }

    private async Task<string> CreateJwtToken(User user)
    {
        var signingCredentials = jwtService.GetSigningCredentials();
        var claims = await jwtService.GetClaimsAsync(user);
        var tokenOptions = jwtService.GenerateTokenOptions(signingCredentials, claims);

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private async Task<bool> AllianceAlreadyExists(int server, string allianceAbbreviation, CancellationToken cancellationToken)
    {
        var allianceToCheck = await context.Alliances
            .FirstOrDefaultAsync(alliance => alliance.Server == server && alliance.Abbreviation.ToLower() == allianceAbbreviation.ToLower(), cancellationToken);

        return allianceToCheck is not null;
    }
}