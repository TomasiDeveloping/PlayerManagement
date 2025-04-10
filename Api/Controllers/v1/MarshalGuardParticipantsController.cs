using Application.DataTransferObjects.MarshalGuardParticipant;
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
    public class MarshalGuardParticipantsController(IMarshalGuardParticipantRepository marshalGuardParticipantRepository, ILogger<MarshalGuardParticipantsController> logger) : ControllerBase
    {
        [HttpGet("{marshalGuardParticipantId:guid}")]
        public async Task<ActionResult<MarshalGuardParticipantDto>> GetMarshalGuardParticipant(
            Guid marshalGuardParticipantId, CancellationToken cancellationToken)
        {
            try
            {
                var marshalGuardParticipantResult =
                    await marshalGuardParticipantRepository.GetMarshalGuardParticipantAsync(marshalGuardParticipantId,
                        cancellationToken);

                return marshalGuardParticipantResult.IsFailure
                    ? BadRequest(marshalGuardParticipantResult.Error)
                    : Ok(marshalGuardParticipantResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetMarshalGuardParticipant)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpGet("Player/{playerId:guid}")]
        public async Task<ActionResult<List<MarshalGuardParticipantDto>>> GetPlayerMarshalGuardsParticipants(Guid playerId, [FromQuery] int last,
            CancellationToken cancellationToken)
        {
            try
            {
                var numberOfParticipationResult =
                    await marshalGuardParticipantRepository.GetPlayerMarshalParticipantsAsync(playerId, last,
                        cancellationToken);
                return numberOfParticipationResult.IsFailure
                    ? BadRequest(numberOfParticipationResult.Error)
                    : Ok(numberOfParticipationResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetPlayerMarshalGuardsParticipants)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertMarshalGuardParticipant(
            List<CreateMarshalGuardParticipantDto> createMarshalGuardParticipantsDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var createResult =
                    await marshalGuardParticipantRepository.InsertMarshalGuardParticipantAsync(
                        createMarshalGuardParticipantsDto, cancellationToken);

                return createResult.IsFailure
                    ? BadRequest(createResult.Error)
                    : StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(InsertMarshalGuardParticipant)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpPut("{marshalGuardParticipantId:guid}")]
        public async Task<ActionResult<MarshalGuardParticipantDto>> UpdateMarshalGuardParticipant(Guid marshalGuardParticipantId,
            UpdateMarshalGuardParticipantDto updateMarshalGuardParticipantDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                if (marshalGuardParticipantId != updateMarshalGuardParticipantDto.Id)
                    return Conflict(MarshalGuardErrors.IdConflict);

                var updateResult =
                    await marshalGuardParticipantRepository.UpdateMarshalGuardParticipantAsync(
                        updateMarshalGuardParticipantDto, cancellationToken);

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
