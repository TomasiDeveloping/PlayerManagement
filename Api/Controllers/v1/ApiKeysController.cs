using Application.DataTransferObjects.ApiKey;
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
    public class ApiKeysController(IApiKeyRepository apiKeyRepository, ILogger<ApiKeysController> logger, IClaimTypeService claimTypeService) : ControllerBase
    {
        [HttpGet("{allianceId:guid}")]
        public async Task<ActionResult<ApiKeyDto>> GetAllianceApiKey(Guid allianceId,
            CancellationToken cancellationToken)
        {
            try
            {
                var allianceApiKeyResult =
                    await apiKeyRepository.GetApiKeyByAllianceIdAsync(allianceId, cancellationToken);

                return allianceApiKeyResult.IsFailure
                    ? BadRequest(allianceApiKeyResult.Error)
                    : Ok(allianceApiKeyResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiKeyDto>> CreateApiKey(CreateApiKeyDto createApiKeyDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var createResult = await apiKeyRepository.CreateApiKeyAsync(createApiKeyDto,
                    claimTypeService.GetFullName(User), cancellationToken);

                return createResult.IsFailure
                    ? BadRequest(createResult.Error)
                    : Ok(createResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{apiKeyId:guid}")]
        public async Task<ActionResult<ApiKeyDto>> UpdateApiKey(Guid apiKeyId, UpdateApiKeyDto updateApiKeyDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                if (updateApiKeyDto.Id != apiKeyId) return Conflict(ApiKeyErrors.IdConflict);

                var updateResult = await apiKeyRepository.UpdateApiKeyAsync(updateApiKeyDto,
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

        [HttpDelete("{apiKeyId:guid}")]
        public async Task<ActionResult<bool>> DeleteApiKey(Guid apiKeyId, CancellationToken cancellationToken)
        {
            try
            {
                var deleteResult = await apiKeyRepository.DeleteApiKeyAsync(apiKeyId, cancellationToken);

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
