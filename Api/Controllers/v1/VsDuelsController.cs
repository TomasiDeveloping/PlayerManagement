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
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("Alliance/{allianceId:guid}")]
        public async Task<ActionResult<List<VsDuelDto>>> GetAllianceVsDuels(Guid allianceId, [FromQuery] int take,
            CancellationToken cancellationToken)
        {
            try
            {
                var allianceVsDuelsResult =
                    await vsDuelRepository.GetAllianceVsDuelsAsync(allianceId, take, cancellationToken);

                if (allianceVsDuelsResult.IsFailure) return BadRequest(allianceVsDuelsResult.Error);

                return allianceVsDuelsResult.Value.Count > 0
                    ? Ok(allianceVsDuelsResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
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
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
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
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
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
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
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
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
