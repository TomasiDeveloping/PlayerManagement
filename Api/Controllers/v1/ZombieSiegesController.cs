using Application.DataTransferObjects;
using Application.DataTransferObjects.ZombieSiege;
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
    public class ZombieSiegesController(IZombieSiegeRepository zombieSiegeRepository, ILogger<ZombieSiegesController> logger, IClaimTypeService claimTypeService) : ControllerBase
    {
        [HttpGet("{zombieSiegeId:guid}")]
        public async Task<ActionResult<ZombieSiegeDto>> GetZombieSiege(Guid zombieSiegeId,
            CancellationToken cancellationToken)
        {
            try
            {
                var zombieSiegeResult =
                    await zombieSiegeRepository.GetZombieSiegeAsync(zombieSiegeId, cancellationToken);

                return zombieSiegeResult.IsFailure
                    ? BadRequest(zombieSiegeResult.Error)
                    : Ok(zombieSiegeResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("Alliance/{allianceId:guid}")]
        public async Task<ActionResult<PagedResponseDto<ZombieSiegeDto>>> GetAllianceZombieSieges(Guid allianceId, CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var allianceZombieSiegesResult =
                    await zombieSiegeRepository.GetAllianceZombieSiegesAsync(allianceId, pageNumber, pageSize,
                        cancellationToken);

                if (allianceZombieSiegesResult.IsFailure) return BadRequest(allianceZombieSiegesResult.Error);

                return allianceZombieSiegesResult.Value.TotalRecords > 0
                    ? Ok(allianceZombieSiegesResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("[action]/{zombieSiegeId:guid}")]
        public async Task<ActionResult<ZombieSiegeDetailDto>> GetZombieSiegeDetail(Guid zombieSiegeId,
            CancellationToken cancellationToken)
        {
            try
            {
                var zombieSiegeDetailResult =
                    await zombieSiegeRepository.GetZombieSiegeDetailAsync(zombieSiegeId, cancellationToken);

                return zombieSiegeDetailResult.IsFailure
                    ? BadRequest(zombieSiegeDetailResult.Error)
                    : Ok(zombieSiegeDetailResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ZombieSiegeDto>> CreateZombieSiege(CreateZombieSiegeDto createZombieSiegeDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var createResult =
                    await zombieSiegeRepository.CreateZombieSiegeAsync(createZombieSiegeDto, claimTypeService.GetFullName(User), cancellationToken);

                return createResult.IsFailure
                    ? BadRequest(createResult.Error)
                    : CreatedAtAction(nameof(GetZombieSiege),
                        new { zombieSiegeId = createResult.Value.Id }, createResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{zombieSiegeId:guid}")]
        public async Task<ActionResult<ZombieSiegeDto>> UpdateZombieSiege(Guid zombieSiegeId, UpdateZombieSiegeDto updateZombieSiegeDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                if (zombieSiegeId != updateZombieSiegeDto.Id) return Conflict(ZombieSiegeErrors.IdConflict);

                var updateResult = await zombieSiegeRepository.UpdateZombieSiegeAsync(updateZombieSiegeDto,
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

        [HttpDelete("{zombieSiegeId:guid}")]
        public async Task<ActionResult<bool>> DeleteZombieSiege(Guid zombieSiegeId, CancellationToken cancellationToken)
        {
            try
            {
                var deleteResult = await zombieSiegeRepository.DeleteZombieSiegeAsync(zombieSiegeId, cancellationToken);

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
