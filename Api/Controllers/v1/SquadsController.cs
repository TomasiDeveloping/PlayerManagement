using Application.DataTransferObjects.Squad;
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
    public class SquadsController(ISquadRepository squadRepository, ILogger<SquadsController> logger) : ControllerBase
    {
        [HttpGet("player/{playerId:guid}")]
        public async Task<ActionResult<List<SquadDto>>> GetPlayerSquads(Guid playerId, CancellationToken cancellationToken)
        {
            try
            {
                var playerSquadsResult = await squadRepository.GetPlayerSquadsAsync(playerId, cancellationToken);

                if (playerSquadsResult.IsFailure) return BadRequest(playerSquadsResult.Error);

                return playerSquadsResult.Value.Count > 0
                    ? Ok(playerSquadsResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetPlayerSquads)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<SquadDto>> CreateSquad(CreateSquadDto createSquadDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var createSquadResult = await squadRepository.CreateSquadAsync(createSquadDto, cancellationToken);

                return createSquadResult.IsSuccess
                    ? Ok(createSquadResult.Value)
                    : BadRequest(createSquadResult.Error);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetPlayerSquads)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpPut("{squadId:guid}")]
        public async Task<ActionResult<SquadDto>> UpdateSquad(Guid squadId, UpdateSquadDto updateSquadDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var updateSquadResult = await squadRepository.UpdateSquadAsync(updateSquadDto, cancellationToken);
                return updateSquadResult.IsSuccess
                    ? Ok(updateSquadResult.Value)
                    : BadRequest(updateSquadResult.Error);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(UpdateSquad)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpDelete("{squadId:guid}")]
        public async Task<ActionResult<bool>> DeleteSquad(Guid squadId, CancellationToken cancellationToken)
        {
            try
            {
                var deleteSquadResult = await squadRepository.DeleteSquadAsync(squadId, cancellationToken);

                return deleteSquadResult.IsSuccess
                    ? Ok(deleteSquadResult.Value)
                    : BadRequest(deleteSquadResult.Error);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(DeleteSquad)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }
    }

}
