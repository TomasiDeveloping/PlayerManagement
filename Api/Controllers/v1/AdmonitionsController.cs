using Application.DataTransferObjects.Admonition;
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
    //[Authorize]
    public class AdmonitionsController(IAdmonitionRepository admonitionRepository, ILogger<AdmonitionsController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<AdmonitionDto>>> GetAdmonitions(CancellationToken cancellationToken)
        {
            try
            {
                var admonitionsResult = await admonitionRepository.GetAdmonitionsAsync(cancellationToken);

                if (admonitionsResult.IsFailure) return BadRequest(admonitionsResult.Error);

                return admonitionsResult.Value.Count > 0
                    ? Ok(admonitionsResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{admonitionId:guid}")]
        public async Task<ActionResult<AdmonitionDto>> GetAdmonition(Guid admonitionId, CancellationToken cancellationToken)
        {
            try
            {
                var admonitionResult = await admonitionRepository.GetAdmonitionAsync(admonitionId, cancellationToken);

                return admonitionResult.IsFailure
                    ? BadRequest(admonitionResult.Error)
                    : Ok(admonitionResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("Player/{playerId:guid}")]
        public async Task<ActionResult<List<AdmonitionDto>>> GetPlayerAdmonitions(Guid playerId, CancellationToken cancellationToken)
        {
            try
            {
                var playerAdmonitionsResult = await admonitionRepository.GetPlayerAdmonitionsAsync(playerId, cancellationToken);

                if (playerAdmonitionsResult.IsFailure) return BadRequest(playerAdmonitionsResult.Error);

                return playerAdmonitionsResult.Value.Count > 0
                    ? Ok(playerAdmonitionsResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult<AdmonitionDto>> CreateAdmonition(CreateAdmonitionDto createAdmonitionDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var createResult =
                    await admonitionRepository.CreateAdmonitionAsync(createAdmonitionDto, cancellationToken);

                return createResult.IsFailure
                    ? BadRequest(createResult.Error)
                    : CreatedAtAction(nameof(GetAdmonition), new { admonitionId = createResult.Value.Id }, createResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{admonitionId:guid}")]
        public async Task<ActionResult<AdmonitionDto>> UpdateAdmonition(Guid admonitionId,
            UpdateAdmonitionDto updateAdmonitionDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                if (admonitionId != updateAdmonitionDto.Id) return Conflict(AdmonitionErrors.IdConflict);

                var updateResult =
                    await admonitionRepository.UpdateAdmonitionAsync(updateAdmonitionDto, cancellationToken);

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

        [HttpDelete("{admonitionId:guid}")]
        public async Task<ActionResult<bool>> DeleteAdmonition(Guid admonitionId, CancellationToken cancellationToken)
        {
            try
            {
                var deleteResult = await admonitionRepository.DeleteAdmonitionAsync(admonitionId, cancellationToken);

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
