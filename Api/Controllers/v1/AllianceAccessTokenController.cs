using Application.DataTransferObjects.AllianceAccessToken;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AllianceAccessTokenController(IAllianceAccessTokenService service,IConfiguration configuration, ILogger<AllianceAccessTokenController> logger) : ControllerBase
    {
        [HttpPost("generate")]
        public async Task<ActionResult<AllianceAccessTokenDto>> GenerateToken(
            [FromBody] CreateAllianceAccessTokenDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var baseUrl = configuration.GetValue<string>("AppSettings:FrontendUrl") ?? throw new NullReferenceException("Settings:FrontendUrl is not configured in appsettings.json");

                var result = await service.GenerateNewTokenAsync(dto, baseUrl, cancellationToken);
                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GenerateToken)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpGet("alliance/{allianceId:guid}")]
        public async Task<ActionResult<IEnumerable<AllianceAccessTokenDto>>> GetTokensForAlliance(Guid allianceId, CancellationToken cancellationToken)
        {
            try
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var tokens = await service.GetTokensForAllianceAsync(allianceId, baseUrl, cancellationToken);
                return Ok(tokens);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetTokensForAlliance)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpPost("revoke/{tokenId:guid}")]
        public async Task<IActionResult> RevokeToken(Guid tokenId, CancellationToken cancellationToken)
        {
            try
            {
                var success = await service.RevokeTokenAsync(tokenId, cancellationToken);
                if (!success) return NotFound();

                return NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(RevokeToken)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }
    }
}
