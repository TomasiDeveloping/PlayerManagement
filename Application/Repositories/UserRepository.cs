using Application.Classes;
using Application.DataTransferObjects.User;
using Application.Errors;
using Application.Interfaces;
using Database;
using Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Repositories;

public class UserRepository(ApplicationContext context, ILogger<UserRepository> logger, UserManager<User> userManager) : IUserRepository
{
    public async Task<Result<List<UserDto>>> GetAllianceUsersAsync(Guid allianceId, CancellationToken cancellationToken)
    {
        var allianceUsers = await context.Users
            .Where(user => user.AllianceId == allianceId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (allianceUsers.Count <= 0) return Result.Success(new List<UserDto>());
        {
            var users = new List<UserDto>();
            foreach (var user in allianceUsers)
            {
                users.Add(new UserDto
                {
                    Id = user.Id,
                    PlayerName = user.PlayerName,
                    Email = user.Email!,
                    AllianceId = user.AllianceId,
                    Role = (await userManager.GetRolesAsync(user)).FirstOrDefault()!
                });
            }

            return Result.Success(users);
        }

    }

    public async Task<Result<UserDto>> GetUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userById = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);

        if (userById is null) return Result.Failure<UserDto>(UserErrors.NotFound);

        var userDto = new UserDto()
        {
            Id = userById.Id,
            Email = userById.Email!,
            PlayerName = userById.PlayerName,
            AllianceId = userById.AllianceId,
            Role = (await userManager.GetRolesAsync(userById)).FirstOrDefault()!
        };

        return Result.Success(userDto);
    }

    public async Task<Result> ChangeUserPasswordAsync(ChangePasswordDto changePasswordDto, CancellationToken cancellationToken)
    {
        var userToChange = await userManager.FindByIdAsync(changePasswordDto.UserId.ToString());

        if (userToChange is null) return Result.Failure(UserErrors.NotFound);

        var checkCurrentPassword =
            await userManager.CheckPasswordAsync(userToChange, changePasswordDto.CurrentPassword);

        if (!checkCurrentPassword) return Result.Failure(UserErrors.CurrentPasswordNotMatch);

        var changePasswordResult = await userManager.ChangePasswordAsync(userToChange,
            changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

        return changePasswordResult.Succeeded
            ? Result.Success()
            : Result.Failure(UserErrors.ChangePasswordFailed);
    }

    public async Task<Result<UserDto>> UpdateUserAsync(UpdateUserDto updateUserDto, CancellationToken cancellationToken)
    {
        var userToUpdate = await userManager.FindByIdAsync(updateUserDto.Id.ToString());

        if (userToUpdate is null) return Result.Failure<UserDto>(UserErrors.NotFound);

        var userRole = (await userManager.GetRolesAsync(userToUpdate)).FirstOrDefault()!;

        if (userRole != updateUserDto.Role)
        {
            await userManager.RemoveFromRoleAsync(userToUpdate, userRole);
            var addRoleResult = await userManager.AddToRoleAsync(userToUpdate, updateUserDto.Role);

            if (!addRoleResult.Succeeded) return Result.Failure<UserDto>(GeneralErrors.DatabaseError);
        }

        try
        {
            userToUpdate.PlayerName = updateUserDto.PlayerName;
            var updateResult = await userManager.UpdateAsync(userToUpdate);

            return updateResult.Succeeded
                ? Result.Success(new UserDto()
                {
                    Email = userToUpdate.Email!,
                    PlayerName = updateUserDto.PlayerName,
                    Role = updateUserDto.Role,
                    AllianceId = userToUpdate.AllianceId,
                    Id = userToUpdate.Id
                })
                : Result.Failure<UserDto>(GeneralErrors.DatabaseError);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result.Failure<UserDto>(GeneralErrors.DatabaseError);
        }
  
    }

    public async Task<Result> DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userToDelete = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);

        if (userToDelete is null) return Result.Failure(UserErrors.NotFound);

        try
        {
            var deleteResult = await userManager.DeleteAsync(userToDelete);

            return deleteResult.Succeeded ? Result.Success() : Result.Failure(GeneralErrors.DatabaseError);
        }
        catch (Exception e)
        {
    
            logger.LogError(e, e.Message);
            return Result.Failure(GeneralErrors.DatabaseError);
        }
 
    }
}