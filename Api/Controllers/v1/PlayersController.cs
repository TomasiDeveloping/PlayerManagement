using Application.DataTransferObjects.Player;
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
    public class PlayersController(IPlayerRepository playerRepository, IClaimTypeService claimTypeService, ILogger<PlayersController> logger) : ControllerBase
    {
        [HttpGet("{playerId:guid}")]
        public async Task<ActionResult<PlayerDto>> GetPlayer(Guid playerId, CancellationToken cancellationToken)
        {
            try
            {
                var playerResult = await playerRepository.GetPlayerAsync(playerId, cancellationToken);

                return playerResult.IsFailure
                    ? BadRequest(playerResult.Error)
                    : Ok(playerResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("Alliance/{allianceId:guid}")]
        public async Task<ActionResult<List<PlayerDto>>> GetAlliancePlayers(Guid allianceId,
            CancellationToken cancellationToken)
        {
            try
            {
                var alliancePlayersResult =
                    await playerRepository.GetAlliancePlayersAsync(allianceId, cancellationToken);

                if (alliancePlayersResult.IsFailure) return BadRequest(alliancePlayersResult.Error);

                return alliancePlayersResult.Value.Count > 0
                    ? Ok(alliancePlayersResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult<PlayerDto>> CreatePlayer(CreatePlayerDto createPlayerDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var createResult = await playerRepository.CreatePlayerAsync(createPlayerDto, claimTypeService.GetFullName(User), cancellationToken);

                return createResult.IsFailure
                    ? BadRequest(createResult.Error)
                    : CreatedAtAction(nameof(GetPlayer), new { playerId = createResult.Value.Id }, createResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{playerId:guid}")]
        public async Task<ActionResult<PlayerDto>> UpdatePlayer(Guid playerId, UpdatePlayerDto updatePlayerDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                if (playerId != updatePlayerDto.Id) return Conflict(PlayerErrors.IdConflict);

                var updateResult = await playerRepository.UpdatePlayerAsync(updatePlayerDto, claimTypeService.GetFullName(User), cancellationToken);

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

        [HttpDelete("{playerId:guid}")]
        public async Task<ActionResult<bool>> DeletePlayer(Guid playerId, CancellationToken cancellationToken)
        {
            try
            {
                var deleteResult = await playerRepository.DeletePlayerAsync(playerId, cancellationToken);

                return deleteResult.IsFailure
                    ? BadRequest(deleteResult.Error)
                    : Ok(deleteResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
