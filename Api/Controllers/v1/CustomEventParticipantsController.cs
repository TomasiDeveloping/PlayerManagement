using Application.DataTransferObjects.CustomEventParticipant;
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
    public class CustomEventParticipantsController(ICustomEventParticipantRepository customEventParticipantRepository, ILogger<CustomEventParticipantsController> logger) : ControllerBase
    {

        [HttpGet("{customEventParticipantId:guid}")]
        public async Task<ActionResult<CustomEventParticipantDto>> GetCustomEventParticipant(
            Guid customEventParticipantId, CancellationToken cancellationToken)
        {
            try
            {
                var customEventParticipantResult =
                    await customEventParticipantRepository.GetCustomEventParticipantAsync(customEventParticipantId,
                        cancellationToken);

                return customEventParticipantResult.IsFailure
                    ? BadRequest(customEventParticipantResult.Error)
                    : Ok(customEventParticipantResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("Player/{playerId:guid}")]
        public async Task<ActionResult<List<CreateCustomEventParticipantDto>>> GetPlayerCustomEventParticipants(
            Guid playerId, [FromQuery] int last,
            CancellationToken cancellationToken)
        {
            try
            {
                var customEventPlayerParticipatedResult =
                    await customEventParticipantRepository.GetPlayerCustomEventParticipantsAsync(playerId, last,
                        cancellationToken);

                if (customEventPlayerParticipatedResult.IsFailure)
                    return BadRequest(customEventPlayerParticipatedResult.Error);

                return customEventPlayerParticipatedResult.Value.Count > 0
                    ? Ok(customEventPlayerParticipatedResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertCustomEventParticipant(
            List<CreateCustomEventParticipantDto> createCustomEventParticipants, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var createResult =
                    await customEventParticipantRepository.InsertCustomEventParticipantAsync(
                        createCustomEventParticipants, cancellationToken);

                return createResult.IsFailure
                    ? BadRequest(createResult.Error)
                    : StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{customEventParticipantId:guid}")]
        public async Task<ActionResult<CustomEventParticipantDto>> UpdateCustomEventParticipant(
            Guid customEventParticipantId,
            UpdateCustomEventParticipantDto updateEventParticipantDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                if (customEventParticipantId != updateEventParticipantDto.Id)
                    return Conflict(new Error("", ""));

                var updateResult =
                    await customEventParticipantRepository.UpdateCustomEventParticipantAsync(
                        updateEventParticipantDto, cancellationToken);

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
