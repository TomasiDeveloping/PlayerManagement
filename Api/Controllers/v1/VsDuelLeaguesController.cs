using Application.DataTransferObjects.VsDuelLeague;
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
    public class VsDuelLeaguesController(IVsDuelLeagueRepository vsDuelLeagueRepository, ILogger<VsDuelLeaguesController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<VsDuelLeagueDto>>> GetVsDuelLeagues(CancellationToken cancellationToken)
        {
            try
            {
                var vsDuelLeaguesResult = await vsDuelLeagueRepository.GetVsDuelLeaguesAsync(cancellationToken);

                if (vsDuelLeaguesResult.IsFailure) return BadRequest(vsDuelLeaguesResult.Error);

                return vsDuelLeaguesResult.Value.Count > 0
                    ? Ok(vsDuelLeaguesResult.Value)
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
