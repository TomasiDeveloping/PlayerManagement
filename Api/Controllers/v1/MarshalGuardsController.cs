using Application.DataTransferObjects.MarshalGuard;
using Application.Errors;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    //[Authorize]
    public class MarshalGuardsController(IMarshalGuardRepository marshalGuardRepository, ILogger<MarshalGuardsController> logger) : ControllerBase
    {
        [HttpGet("{marshalGuardId:guid}")]
        public async Task<ActionResult<MarshalGuardDto>> GetMarshalGuard(Guid marshalGuardId,
            CancellationToken cancellationToken)
        {
            try
            {
                var marshalGuardResult =
                    await marshalGuardRepository.GetMarshalGuardAsync(marshalGuardId, cancellationToken);

                return marshalGuardResult.IsFailure
                    ? BadRequest(marshalGuardResult.Error)
                    : Ok(marshalGuardResult.Value);
            }
            catch (Exception e)
            { 
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("Player/{playerId:guid}")]
        public async Task<ActionResult<List<MarshalGuardDto>>> GetPlayerMarshalGuards(Guid playerId,
            CancellationToken cancellationToken)
        {
            try
            {
                var playerMarshalGuardsResult =
                    await marshalGuardRepository.GetPlayerMarshalGuardsAsync(playerId, cancellationToken);

                if (playerMarshalGuardsResult.IsFailure) return BadRequest(playerMarshalGuardsResult.Error);

                return playerMarshalGuardsResult.Value.Count > 0
                    ? Ok(playerMarshalGuardsResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult<MarshalGuardDto>> CreateMarshalGuard(CreateMarshalGuardDto createMarshalGuardDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var createResult =
                    await marshalGuardRepository.CreateMarshalGuardAsync(createMarshalGuardDto, cancellationToken);

                return createResult.IsFailure
                    ? BadRequest(createResult.Error)
                    : CreatedAtAction(nameof(GetMarshalGuard), new { marshalGuardId = createResult.Value.Id },
                        createResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); ;
            }
        }

        [HttpPut("{marshalGuardId:guid}")]
        public async Task<ActionResult<MarshalGuardDto>> UpdateMarshalGuard(Guid marshalGuardId,
            UpdateMarshalGuardDto updateMarshalGuardDto, CancellationToken cancellationToken)
        {
            try
            {
                if (ModelState.IsValid) return UnprocessableEntity(ModelState);

                if (marshalGuardId != updateMarshalGuardDto.Id) return Conflict(MarshalGuardErrors.IdConflict);

                var updateResult =
                    await marshalGuardRepository.UpdateMarshalGuardAsync(updateMarshalGuardDto, cancellationToken);

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

        [HttpDelete("{marshalGuardId:guid}")]
        public async Task<ActionResult<bool>> DeleteMarshalGuard(Guid marshalGuardId,
            CancellationToken cancellationToken)
        {
            try
            {
                var deleteResult =
                    await marshalGuardRepository.DeleteMarshalGuardAsync(marshalGuardId, cancellationToken);

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
