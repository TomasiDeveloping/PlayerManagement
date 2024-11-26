using Application.Classes;
using Application.DataTransferObjects.User;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<Result<List<UserDto>>> GetAllianceUsersAsync(Guid allianceId, CancellationToken cancellationToken);

    Task<Result<UserDto>> GetUserAsync(Guid userId, CancellationToken cancellationToken);

    Task<Result> ChangeUserPasswordAsync(ChangePasswordDto  changePasswordDto, CancellationToken cancellationToken);

    Task<Result<UserDto>> UpdateUserAsync(UpdateUserDto  updateUserDto, CancellationToken cancellationToken);

    Task<Result> DeleteUserAsync(Guid userId, CancellationToken cancellationToken);
}