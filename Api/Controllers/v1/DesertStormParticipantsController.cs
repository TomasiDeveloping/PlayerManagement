using Application.DataTransferObjects.DesertStormParticipants;
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
    public class DesertStormParticipantsController(IDesertStormParticipantRepository desertStormParticipantRepository, ILogger<DesertStormParticipantsController> logger) : ControllerBase
    {
        [HttpGet("{desertStormParticipantId:guid}")]
        public async Task<ActionResult<DesertStormParticipantDto>> GetDesertStormParticipant(
            Guid desertStormParticipantId, CancellationToken cancellationToken)
        {
            try
            {
                var desertStormParticipantResult =
                    await desertStormParticipantRepository.GetDesertStormParticipantAsync(desertStormParticipantId,
                        cancellationToken);

                return desertStormParticipantResult.IsFailure
                    ? BadRequest(desertStormParticipantResult.Error)
                    : Ok(desertStormParticipantResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetDesertStormParticipant)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpGet("Player/{playerId:guid}")]
        public async Task<ActionResult<List<DesertStormParticipantDto>>> GetPlayerDesertStormParticipants(
            Guid playerId, [FromQuery] int last,
            CancellationToken cancellationToken)
        {
            try
            {
                var desertStormPlayerParticipatedResult =
                    await desertStormParticipantRepository.GetPlayerDesertStormParticipantsAsync(playerId, last,
                        cancellationToken);

                if (desertStormPlayerParticipatedResult.IsFailure)
                    return BadRequest(desertStormPlayerParticipatedResult.Error);

                return desertStormPlayerParticipatedResult.Value.Count > 0
                    ? Ok(desertStormPlayerParticipatedResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetPlayerDesertStormParticipants)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertDesertStormParticipant(
            List<CreateDesertStormParticipantDto> createDesertStormParticipantsDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var createResult =
                    await desertStormParticipantRepository.InsertDesertStormParticipantAsync(
                        createDesertStormParticipantsDto, cancellationToken);

                return createResult.IsFailure
                    ? BadRequest(createResult.Error)
                    : StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(InsertDesertStormParticipant)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpPut("{desertStormParticipantId:guid}")]
        public async Task<ActionResult<DesertStormParticipantDto>> UpdateMarshalGuardParticipant(
            Guid desertStormParticipantId,
            UpdateDesertStormParticipantDto updateDesertStormParticipantDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                if (desertStormParticipantId != updateDesertStormParticipantDto.Id)
                    return Conflict(new Error("",""));

                var updateResult =
                    await desertStormParticipantRepository.UpdateDesertStormParticipantAsync(
                        updateDesertStormParticipantDto, cancellationToken);

                return updateResult.IsFailure
                    ? BadRequest(updateResult.Error)
                    : Ok(updateResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(UpdateMarshalGuardParticipant)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }
    }
}
