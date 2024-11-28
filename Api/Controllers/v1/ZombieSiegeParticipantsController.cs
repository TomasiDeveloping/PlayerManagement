using Application.DataTransferObjects.ZombieSiegeParticipant;
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
    public class ZombieSiegeParticipantsController(IZombieSiegeParticipantRepository zombieSiegeParticipantRepository, ILogger<ZombieSiegeParticipantsController> logger) : ControllerBase
    {
        [HttpGet("{zombieSiegeParticipantId:guid}")]
        public async Task<ActionResult<ZombieSiegeParticipantDto>> GetZombieSiegeParticipant(
            Guid zombieSiegeParticipantId, CancellationToken cancellationToken)
        {
            try
            {
                var zombieSiegeParticipantResult =
                    await zombieSiegeParticipantRepository.GetZombieSiegeParticipantAsync(zombieSiegeParticipantId,
                        cancellationToken);

                return zombieSiegeParticipantResult.IsFailure
                    ? BadRequest(zombieSiegeParticipantResult.Error)
                    : Ok(zombieSiegeParticipantResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("Player/{playerId:guid}")]
        public async Task<ActionResult<List<ZombieSiegeParticipantDto>>> GetPlayerZombieSiegeParticipants(Guid playerId, [FromQuery] int last,
            CancellationToken cancellationToken)
        {
            try
            {
                var playerZombieSiegeParticipantsResult =
                    await zombieSiegeParticipantRepository.GetPlayerZombieSiegeParticipantsAsync(playerId, last,
                        cancellationToken);

                return playerZombieSiegeParticipantsResult.IsFailure
                    ? BadRequest(playerZombieSiegeParticipantsResult.Error)
                    : Ok(playerZombieSiegeParticipantsResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> InsertZombieSiegeParticipants(
            List<CreateZombieSiegeParticipantDto> createZombieSiegeParticipants, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var createResult =
                    await zombieSiegeParticipantRepository.InsertZombieSiegeParticipantsAsync(
                        createZombieSiegeParticipants, cancellationToken);

                return createResult.IsFailure
                    ? BadRequest(createResult.Error)
                    : Ok(createResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{zombieSiegeParticipantId:guid}")]
        public async Task<ActionResult<ZombieSiegeParticipantDto>> UpdateZombieSiegeParticipant(
            Guid zombieSiegeParticipantId, UpdateZombieSiegeParticipantDto updateZombieSiegeParticipant,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                if (zombieSiegeParticipantId != updateZombieSiegeParticipant.Id)
                    return Conflict(ZombieSiegeParticipantErrors.IdConflict);

                var updateResult =
                    await zombieSiegeParticipantRepository.UpdateZombieSiegeParticipantAsync(
                        updateZombieSiegeParticipant, cancellationToken);

                return updateResult.IsFailure
                    ? BadRequest(updateResult.Error)
                    : Ok(updateResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
