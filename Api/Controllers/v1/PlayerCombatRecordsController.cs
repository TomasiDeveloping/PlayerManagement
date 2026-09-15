using Application.DataTransferObjects.PlayerCombatRecord;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class PlayerCombatRecordsController(IPlayerCombatRecordService service, ILogger<PlayerCombatRecordsController> logger) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreatePlayerCombatRecord([FromBody] SubmitPlayerCombatRecordDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await service.SubmitRecordAsync(dto, cancellationToken);
                return Ok();
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(CreatePlayerCombatRecord)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }
    }
}
