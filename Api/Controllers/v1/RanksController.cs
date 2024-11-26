using Application.DataTransferObjects.Rank;
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
    public class RanksController(IRankRepository rankRepository, ILogger<RanksController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<RankDto>>> GetRanks(CancellationToken cancellationToken)
        {
            try
            {
                var ranksResult = await rankRepository.GetRanksAsync(cancellationToken);

                if (ranksResult.IsFailure) return BadRequest(ranksResult.Error);

                return ranksResult.Value.Count > 0
                    ? Ok(ranksResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
