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
        public async Task<ActionResult<LoginResponseDto>> Register(RegisterRequestDto registerRequestDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var registerResult =
                    await authenticationRepository.RegisterToApplicationAsync(registerRequestDto, cancellationToken);

                return registerResult.IsFailure
                    ? BadRequest(registerResult.Error)
                    : Ok(registerResult.Value);
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
    }
}
