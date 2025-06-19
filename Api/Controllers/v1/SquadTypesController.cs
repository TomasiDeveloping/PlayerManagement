using Application.DataTransferObjects.SquadType;
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
    public class SquadTypesController(ISquadTypeRepository squadTypeRepository, ILogger<SquadTypesController> logger)
        : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<SquadTypeDto>>> GetSquadTypes(CancellationToken cancellationToken)
        {
            try
            {
                var squadTypesResult = await squadTypeRepository.GetSquadTypesAsync(cancellationToken);
                if (squadTypesResult.IsFailure) return BadRequest(squadTypesResult.Error);
                return squadTypesResult.Value.Count > 0
                    ? Ok(squadTypesResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetSquadTypes)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }
    }
}
