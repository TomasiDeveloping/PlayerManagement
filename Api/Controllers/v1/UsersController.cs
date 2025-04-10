using Application.DataTransferObjects.User;
using Application.Errors;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class UsersController(IUserRepository userRepository, ILogger<UsersController> logger) : ControllerBase
    {
        [HttpGet("Alliance/{allianceId:guid}")]
        public async Task<ActionResult<List<UserDto>>> GetAllianceUsers(Guid allianceId,
            CancellationToken cancellationToken)
        {
            try
            {
                var allianceUsersResult = await userRepository.GetAllianceUsersAsync(allianceId, cancellationToken);

                if (allianceUsersResult.IsFailure) return BadRequest(allianceUsersResult.Error);

                return allianceUsersResult.Value.Count > 0
                    ? Ok(allianceUsersResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetAllianceUsers)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                var userResult = await userRepository.GetUserAsync(userId, cancellationToken);

                return userResult.IsFailure
                    ? BadRequest(userResult.Error)
                    : Ok(userResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetUser)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpPut("{userId:guid}")]
        public async Task<ActionResult<UserDto>> UpdateUser(Guid userId, UpdateUserDto updateUserDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                if (userId != updateUserDto.Id) return Conflict(UserErrors.IdConflict);

                var updateResult = await userRepository.UpdateUserAsync(updateUserDto, cancellationToken);

                return updateResult.IsFailure
                    ? BadRequest(updateResult.Error)
                    : Ok(updateResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(UpdateUser)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> ChangeUserPassword(ChangePasswordDto changePasswordDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword)
                    return BadRequest(UserErrors.ConfirmPasswordNotMatch);

                var changePasswordResult =
                    await userRepository.ChangeUserPasswordAsync(changePasswordDto, cancellationToken);

                return changePasswordResult.IsFailure
                    ? BadRequest(changePasswordResult.Error)
                    : Ok(true);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(ChangeUserPassword)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpDelete("{userId:guid}")]
        public async Task<ActionResult<bool>> DeleteUser(Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                var deleteResult = await userRepository.DeleteUserAsync(userId, cancellationToken);

                return deleteResult.IsFailure
                    ? BadRequest(deleteResult.Error)
                    : Ok(true);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(DeleteUser)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }
    }
}
