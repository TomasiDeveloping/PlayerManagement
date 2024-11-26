using Application.DataTransferObjects.Alliance;
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
    public class AlliancesController(IAllianceRepository allianceRepository, IClaimTypeService claimTypeService, ILogger<AlliancesController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<AllianceDto>>> GetAlliances(CancellationToken cancellationToken)
        {
            try
            {
                var alliancesResult = await allianceRepository.GetAlliancesAsync(cancellationToken);

                if (alliancesResult.IsFailure) return BadRequest(alliancesResult.Error);

                return alliancesResult.Value.Count > 0 
                    ? Ok(alliancesResult.Value) 
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{allianceId:guid}")]
        public async Task<ActionResult<AllianceDto>> GetAlliance(Guid allianceId, CancellationToken cancellationToken)
        {
            try
            {
                var allianceResult = await allianceRepository.GetAllianceAsync(allianceId, cancellationToken);

                return allianceResult.IsFailure
                    ? BadRequest(allianceResult.Error)
                    : Ok(allianceResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{allianceId:guid}")]
        public async Task<ActionResult<AllianceDto>> UpdateAlliance(Guid allianceId, UpdateAllianceDto updateAllianceDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                if (allianceId != updateAllianceDto.Id) return Conflict(AllianceErrors.IdConflict);

                var updateResult = await allianceRepository.UpdateAllianceAsync(updateAllianceDto, claimTypeService.GetFullName(User), cancellationToken);

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

        [HttpDelete("{allianceId:guid}")]
        public async Task<ActionResult<bool>> DeleteAlliance(Guid allianceId, CancellationToken cancellationToken)
        {
            try
            {
                var deleteResult = await allianceRepository.DeleteAllianceAsync(allianceId, cancellationToken);
                
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
