using Application.DataTransferObjects.CustomEventCategory;
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
    public class CustomEventCategoriesController(ICustomEventCategoryRepository customEventCategoryRepository, ILogger<CustomEventCategoriesController> logger) : ControllerBase
    {
        [HttpGet("{customEventCategoryId:guid}")]
        public async Task<ActionResult<CustomEventCategoryDto>> GetCustomEventCategory(Guid customEventCategoryId, CancellationToken cancellationToken)
        {
            try
            {
                var customEventCategoryResult =
                    await customEventCategoryRepository.GetCustomEventCategoryAsync(customEventCategoryId, cancellationToken);

                return customEventCategoryResult.IsFailure
                    ? BadRequest(customEventCategoryResult.Error)
                    : Ok(customEventCategoryResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetCustomEventCategory)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpGet("Alliance/{allianceId:guid}")]
        public async Task<ActionResult<List<CustomEventCategoryDto>>> GetAllianceCustomEventCategories(Guid allianceId, CancellationToken cancellationToken)
        {
            try
            {
                var allianceCustomEventCategoriesResult =
                    await customEventCategoryRepository.GetAllianceCustomEventCategoriesAsync(allianceId, cancellationToken);

                if (allianceCustomEventCategoriesResult.IsFailure) return BadRequest(allianceCustomEventCategoriesResult.Error);

                return allianceCustomEventCategoriesResult.Value.Count > 0
                    ? Ok(allianceCustomEventCategoriesResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetAllianceCustomEventCategories)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CustomEventCategoryDto>> CreateCustomEventCategory(CreateCustomEventCategoryDto createCustomEventCategoryDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var createResult = await customEventCategoryRepository.CreateCustomEventCategoryAsync(createCustomEventCategoryDto, cancellationToken);

                return createResult.IsFailure
                    ? BadRequest(createResult.Error)
                    : CreatedAtAction(nameof(GetCustomEventCategory), new { customEventCategoryId = createResult.Value.Id },
                        createResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(CreateCustomEventCategory)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpPut("{customEventCategoryId:guid}")]
        public async Task<ActionResult<CustomEventCategoryDto>> UpdateCustomEventCategory(Guid customEventCategoryId,
            UpdateCustomEventCategoryDto updateCustomEventCategoryDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                if (updateCustomEventCategoryDto.Id != customEventCategoryId) return Conflict(CustomEventCategoryErrors.IdConflict);

                var updateResult = await customEventCategoryRepository.UpdateCustomEventCategoryAsync(updateCustomEventCategoryDto, cancellationToken);

                return updateResult.IsFailure
                    ? BadRequest(updateResult.Error)
                    : Ok(updateResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(UpdateCustomEventCategory)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpDelete("{customEventCategoryId:guid}")]
        public async Task<ActionResult<bool>> DeleteCustomEventCategory(Guid customEventCategoryId, CancellationToken cancellationToken)
        {
            try
            {
                var deleteResult = await customEventCategoryRepository.DeleteCustomEventAsync(customEventCategoryId, cancellationToken);

                return deleteResult.IsFailure
                    ? BadRequest(deleteResult.Error)
                    : Ok(deleteResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(DeleteCustomEventCategory)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }
    }
}
