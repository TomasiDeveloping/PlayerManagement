using Application.DataTransferObjects.Stat;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class StatsController(IStatRepository statRepository, ILogger<StatsController> logger) : ControllerBase
    {
        [HttpGet("useCount")]
        public async Task<ActionResult<AllianceUseToolCount>> GetUseCount(CancellationToken cancellationToken)
        {
            try
            {
                var useCountResult = await statRepository.GetAllianceUseToolCountAsync(cancellationToken);
                
                return useCountResult.IsFailure
                    ? BadRequest(useCountResult.Error)
                    : Ok(useCountResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetUseCount)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

    }
}
