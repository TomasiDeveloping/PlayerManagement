using Application.DataTransferObjects.DesertStorm;
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
    public class DesertStormsController(IDesertStormRepository desertStormRepository, IClaimTypeService claimTypeService, ILogger<DesertStormsController> logger) : ControllerBase
    {
        [HttpGet("{desertStormId:guid}")]
        public async Task<ActionResult<DesertStormDto>> GetDesertStorm(Guid desertStormId,
            CancellationToken cancellationToken)
        {
            try
            {
                var desertStormResult =
                    await desertStormRepository.GetDesertStormAsync(desertStormId, cancellationToken);

                return desertStormResult.IsFailure
                    ? BadRequest(desertStormResult.Error)
                    : Ok(desertStormResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetDesertStorm)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpGet("Alliance/{allianceId:guid}")]
        public async Task<ActionResult<List<DesertStormDto>>> GetAllianceDesertStorms(Guid allianceId, CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var allianceDesertStormsResult =
                    await desertStormRepository.GetAllianceDesertStormsAsync(allianceId, pageNumber, pageSize, cancellationToken);

                if (allianceDesertStormsResult.IsFailure) return BadRequest(allianceDesertStormsResult.Error);

                return allianceDesertStormsResult.Value.Data.Count > 0
                    ? Ok(allianceDesertStormsResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetAllianceDesertStorms)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpGet("[action]/{desertStormId:guid}")]
        public async Task<ActionResult<DesertStormDetailDto>> GetDesertStormDetail(Guid desertStormId,
            CancellationToken cancellationToken)
        {
            try
            {
                var desertStormDetailResult =
                    await desertStormRepository.GetDesertStormDetailAsync(desertStormId, cancellationToken);

                return desertStormDetailResult.IsFailure
                    ? BadRequest(desertStormDetailResult.Error)
                    : Ok(desertStormDetailResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetDesertStormDetail)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<DesertStormDto>> CreateDesertStorm(CreateDesertStormDto createDesertStormDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var createResult =
                    await desertStormRepository.CreateDesertStormAsync(createDesertStormDto, claimTypeService.GetFullName(User), cancellationToken);

                return createResult.IsFailure
                    ? BadRequest(createResult.Error)
                    : CreatedAtAction(nameof(GetDesertStorm), new { desertStormId = createResult.Value.Id },
                        createResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(CreateDesertStorm)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpPut("{desertStormId:guid}")]
        public async Task<ActionResult<DesertStormDto>> UpdateDesertStorm(Guid desertStormId,
            UpdateDesertStormDto updateDesertStormDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                if (desertStormId != updateDesertStormDto.Id) return Conflict(DesertStormErrors.IdConflict);

                var updateResult =
                    await desertStormRepository.UpdateDesertStormAsync(updateDesertStormDto, claimTypeService.GetFullName(User), cancellationToken);

                return updateResult.IsFailure
                    ? BadRequest(updateResult.Error)
                    : Ok(updateResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(UpdateDesertStorm)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpDelete("{desertStormId:guid}")]
        public async Task<ActionResult<bool>> DeleteDesertStorm(Guid desertStormId, CancellationToken cancellationToken)
        {
            try
            {
                var deleteResult = await desertStormRepository.DeleteDesertStormAsync(desertStormId, cancellationToken);

                return deleteResult.IsFailure
                    ? BadRequest(deleteResult.Error)
                    : Ok(deleteResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(DeleteDesertStorm)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }
    }
}
