using Application.DataTransferObjects.CustomEvent;
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
    public class CustomEventsController(ICustomEventRepository customEventRepository, IClaimTypeService claimTypeService, ILogger<CustomEventsController> logger) : ControllerBase
    {
        [HttpGet("{customEventId:guid}")]
        public async Task<ActionResult<CustomEventDto>> GetCustomEvent(Guid customEventId,
            CancellationToken cancellationToken)
        {
            try
            {
                var customEventResult =
                    await customEventRepository.GetCustomEventAsync(customEventId, cancellationToken);

                return customEventResult.IsFailure
                    ? BadRequest(customEventResult.Error)
                    : Ok(customEventResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("Alliance/{allianceId:guid}")]
        public async Task<ActionResult<List<CustomEventDto>>> GetAllianceCustomEvents(Guid allianceId, CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var allianceCustomEventsResult =
                    await customEventRepository.GetAllianceCustomEventsAsync(allianceId, pageNumber, pageSize, cancellationToken);

                if (allianceCustomEventsResult.IsFailure) return BadRequest(allianceCustomEventsResult.Error);

                return allianceCustomEventsResult.Value.Data.Count > 0
                    ? Ok(allianceCustomEventsResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("[action]/{customEventId:guid}")]
        public async Task<ActionResult<CustomEventDetailDto>> GetCustomEventDetail(Guid customEventId,
            CancellationToken cancellationToken)
        {
            try
            {
                var customEventDetailResult =
                    await customEventRepository.GetCustomEventDetailAsync(customEventId, cancellationToken);

                return customEventDetailResult.IsFailure
                    ? BadRequest(customEventDetailResult.Error)
                    : Ok(customEventDetailResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CustomEventDto>> CreateCustomEvent(CreateCustomEventDto createCustomEventDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var createResult = await customEventRepository.CreateCustomEventAsync(createCustomEventDto,
                    claimTypeService.GetFullName(User), cancellationToken);

                return createResult.IsFailure
                    ? BadRequest(createResult.Error)
                    : CreatedAtAction(nameof(GetCustomEvent), new { customEventId = createResult.Value.Id },
                        createResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{customEventId:guid}")]
        public async Task<ActionResult<CustomEventDto>> UpdateCustomEvent(Guid customEventId,
            UpdateCustomEventDto updateCustomEventDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                if (updateCustomEventDto.Id != customEventId) return Conflict(CustomEventErrors.IdConflict);

                var updateResult = await customEventRepository.UpdateCustomEventAsync(updateCustomEventDto,
                    claimTypeService.GetFullName(User), cancellationToken);

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

        [HttpDelete("{customEventId:guid}")]
        public async Task<ActionResult<bool>> DeleteCustomEvent(Guid customEventId, CancellationToken cancellationToken)
        {
            try
            {
                var deleteResult = await customEventRepository.DeleteCustomEventAsync(customEventId, cancellationToken);

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
