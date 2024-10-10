using System.IdentityModel.Tokens.Jwt;
using Application.Classes;
using Application.DataTransferObjects.Authentication;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using Database;
using Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Utilities.Constants;

namespace Application.Repositories;

public class AuthenticationRepository(UserManager<User> userManager, ApplicationContext context, IMapper mapper, IJwtService jwtService, ILogger<AuthenticationRepository> logger) : IAuthenticationRepository
{
    public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken)
    {
        var userToLogin = await userManager.FindByEmailAsync(loginRequestDto.Email);

        if (userToLogin is null || !await userManager.CheckPasswordAsync(userToLogin, loginRequestDto.Password))
        {
            return Result.Failure<LoginResponseDto>(AuthenticationErrors.LoginFailed);
        }

        var loginResponse = new LoginResponseDto()
        {
            Token = await CreateJwtToken(userToLogin)
        };

        return Result.Success(loginResponse);
    }

    public async Task<Result<LoginResponseDto>> RegisterToApplicationAsync(RegisterRequestDto registerRequestDto, CancellationToken cancellationToken)
    {
        var newUser = mapper.Map<User>(registerRequestDto);
        var userAlliance = mapper.Map<Alliance>(registerRequestDto);

        var checkAllianceExists = await AllianceAlreadyExists(userAlliance.Server, userAlliance.Abbreviation, cancellationToken);

        if (checkAllianceExists) return Result.Failure<LoginResponseDto>(AuthenticationErrors.AllianceAlreadyExists);

        var createAllianceResult = await CreateAlliance(userAlliance, cancellationToken);

        if (createAllianceResult.IsFailure) return Result.Failure<LoginResponseDto>(createAllianceResult.Error);

        newUser.AllianceId = createAllianceResult.Value.Id;

        var userCreateResult = await userManager.CreateAsync(newUser, registerRequestDto.Password);

        if (!userCreateResult.Succeeded)
        {
            var rollBackResult = await RollbackAlliance(createAllianceResult.Value, cancellationToken);

            return Result.Failure<LoginResponseDto>(rollBackResult.IsFailure ? rollBackResult.Error : AuthenticationErrors.RegisterFailed);
        }

        var addRoleResult = await userManager.AddToRoleAsync(newUser, ApplicationRoles.Administrator);

        if (!addRoleResult.Succeeded)
        {
            await userManager.DeleteAsync(newUser);
            await RollbackAlliance(createAllianceResult.Value, cancellationToken);
            return Result.Failure<LoginResponseDto>(AuthenticationErrors.RegisterFailed);
        }

        var response = new LoginResponseDto()
        {
            Token = await CreateJwtToken(newUser)
        };

        return Result.Success(response);
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
            logger.LogError(e, e.Message);
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
            logger.LogError(e, e.Message);
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
            .FirstOrDefaultAsync(alliance => alliance.Server == server && alliance.Abbreviation == allianceAbbreviation, cancellationToken);

        return allianceToCheck is not null;
    }
}