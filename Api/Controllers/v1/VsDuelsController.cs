using Application.DataTransferObjects;
using Application.DataTransferObjects.VsDuel;
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
    public class VsDuelsController(IVsDuelRepository vsDuelRepository, IClaimTypeService claimTypeService, ILogger<VsDuelsController> logger) : ControllerBase
    {
        [HttpGet("{vsDuelId:guid}")]
        public async Task<ActionResult<VsDuelDto>> GetVsDuel(Guid vsDuelId, CancellationToken cancellationToken)
        {
            try
            {
                var vsDuelResult = await vsDuelRepository.GetVsDuelAsync(vsDuelId, cancellationToken);

                return vsDuelResult.IsFailure
                    ? BadRequest(vsDuelResult.Error)
                    : Ok(vsDuelResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetVsDuel)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpGet("Alliance/{allianceId:guid}")]
        public async Task<ActionResult<PagedResponseDto<VsDuelDto>>> GetAllianceVsDuels(Guid allianceId, CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var allianceVsDuelsResult =
                    await vsDuelRepository.GetAllianceVsDuelsAsync(allianceId, pageNumber, pageSize, cancellationToken);

                if (allianceVsDuelsResult.IsFailure) return BadRequest(allianceVsDuelsResult.Error);

                return allianceVsDuelsResult.Value.Data.Count > 0
                    ? Ok(allianceVsDuelsResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetAllianceVsDuels)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpGet("[action]/{vsDuelId:guid}")]
        public async Task<ActionResult<VsDuelDetailDto>> GetDetailVsDuel(Guid vsDuelId, CancellationToken cancellationToken)
        {
            try
            {
                var vsDuelDetailResult = await vsDuelRepository.GetVsDuelDetailAsync(vsDuelId, cancellationToken);

                return vsDuelDetailResult.IsFailure
                    ? BadRequest(vsDuelDetailResult.Error)
                    : Ok(vsDuelDetailResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetDetailVsDuel)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }


        [HttpPost]
        public async Task<ActionResult<VsDuelDto>> CreateVsDuel(CreateVsDuelDto createVsDuelDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var createResult = await vsDuelRepository.CreateVsDuelAsync(createVsDuelDto, claimTypeService.GetFullName(User), cancellationToken);

                return createResult.IsFailure
                    ? BadRequest(createResult.Error)
                    : CreatedAtAction(nameof(GetVsDuel), new { vsDuelId = createResult.Value.Id }, createResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(CreateVsDuel)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpPut("{vsDuelId:guid}")]
        public async Task<ActionResult<VsDuelDto>> UpdateVsDuel(Guid vsDuelId, UpdateVsDuelDto updateVsDuelDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                if (vsDuelId != updateVsDuelDto.Id) return Conflict(VsDuelErrors.IdConflict);

                var updateResult = await vsDuelRepository.UpdateVsDuelAsync(updateVsDuelDto, claimTypeService.GetFullName(User), cancellationToken);

                return updateResult.IsFailure
                    ? BadRequest(updateResult.Error)
                    : Ok(updateResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(UpdateVsDuel)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpDelete("{vsDuelId:guid}")]
        public async Task<ActionResult<bool>> DeleteVsDuel(Guid vsDuelId, CancellationToken cancellationToken)
        {
            try
            {
                var deleteResult = await vsDuelRepository.DeleteVsDuelAsync(vsDuelId, cancellationToken);

                return deleteResult.IsFailure
                    ? BadRequest(deleteResult.Error)
                    : Ok(deleteResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(DeleteVsDuel)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }
    }
}
