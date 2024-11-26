using Application.DataTransferObjects.Authentication;
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
    public class AuthenticationsController(IAuthenticationRepository authenticationRepository, ILogger<AuthenticationsController> logger) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> SignUp(SignUpRequestDto signUpRequestDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var registerResult =
                    await authenticationRepository.RegisterToApplicationAsync(signUpRequestDto, cancellationToken);

                return registerResult.IsFailure
                    ? BadRequest(registerResult.Error)
                    : Ok(true);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> RegisterUser(RegisterUserDto registerUserDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var registerResult =
                    await authenticationRepository.RegisterUserAsync(registerUserDto, cancellationToken);

                return registerResult.IsFailure
                    ? BadRequest(registerResult.Error)
                    : Ok(true);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto loginRequestDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var loginResult = await authenticationRepository.LoginAsync(loginRequestDto, cancellationToken);

                return loginResult.IsFailure
                    ? Unauthorized(loginResult.Error)
                    : Ok(loginResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> ResendConfirmationEmail(
            EmailConfirmationRequestDto emailConfirmationRequestDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var resendConfirmationEmailResult =
                    await authenticationRepository.ResendConfirmationEmailAsync(emailConfirmationRequestDto);

                return resendConfirmationEmailResult.IsFailure
                    ? BadRequest(resendConfirmationEmailResult.Error)
                    : Ok(true);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> ConfirmEmail(ConfirmEmailRequestDto confirmEmailRequestDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var resendConfirmationEmailResult =
                    await authenticationRepository.EmailConfirmationAsync(confirmEmailRequestDto);

                return resendConfirmationEmailResult.IsFailure
                    ? BadRequest(resendConfirmationEmailResult.Error)
                    : Ok(true);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> InviteUser(InviteUserDto inviteUserDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var inviteUserResult = await authenticationRepository.InviteUserAsync(inviteUserDto, cancellationToken);

                if (inviteUserResult.IsFailure) return BadRequest(inviteUserResult.Error);

                return Ok(true);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> ForgotPassword(ForgotPasswordDto forgotPasswordDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var forgotPasswordResponse = await authenticationRepository.ForgotPasswordAsync(forgotPasswordDto);

                if (forgotPasswordResponse.IsFailure)
                {
                    logger.LogWarning(forgotPasswordResponse.Error.Name);
                }
                return Ok(true);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> ResetPassword(ResetPasswordDto resetPasswordDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                if (resetPasswordDto.Password != resetPasswordDto.ConfirmPassword)
                    return BadRequest("Reset password failed");

                var resetPasswordResult = await authenticationRepository.ResetPasswordAsync(resetPasswordDto);

                return resetPasswordResult.IsFailure
                    ? BadRequest(resetPasswordResult.Error)
                    : Ok(true);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
