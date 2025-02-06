using Application.Classes;
using Application.DataTransferObjects.ApiKey;
using Database.Entities;

namespace Application.Interfaces;

public interface IApiKeyRepository
{
    Task<Result<ApiKey>> GetAllianceApiKeyAsync(Guid allianceId);

    Task<Result<ApiKeyDto>> GetApiKeyByAllianceIdAsync(Guid  allianceId, CancellationToken cancellationToken);

    Task<Result<ApiKeyDto>> CreateApiKeyAsync(CreateApiKeyDto createApiKeyDto, string creator, CancellationToken  cancellationToken);

    Task<Result<ApiKeyDto>> UpdateApiKeyAsync(UpdateApiKeyDto updateApiKeyDto, string modifier, CancellationToken cancellationToken);

    Task<Result<bool>> DeleteApiKeyAsync(Guid apiKeyId, CancellationToken cancellationToken);
}