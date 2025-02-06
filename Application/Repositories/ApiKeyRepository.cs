using Application.Classes;
using Application.DataTransferObjects.ApiKey;
using Application.Errors;
using Application.Interfaces;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Repositories;

public class ApiKeyRepository(ApplicationContext context, ILogger<ApiKeyRepository> logger, IEncryptionService encryptionService) : IApiKeyRepository
{
    public async Task<Result<ApiKey>> GetAllianceApiKeyAsync(Guid allianceId)
    {
        var allianceApiKey = await context.ApiKeys
            .AsNoTracking()
            .FirstOrDefaultAsync(apiKey => apiKey.AllianceId == allianceId);

        return allianceApiKey is null
            ? Result.Failure<ApiKey>(ApiKeyErrors.NoKeyForAlliance)
            : Result.Success(allianceApiKey);
    }

    public async Task<Result<ApiKeyDto>> GetApiKeyByAllianceIdAsync(Guid allianceId, CancellationToken cancellationToken)
    {
        var allianceApiKey = await context.ApiKeys
            .AsNoTracking()
            .FirstOrDefaultAsync(apiKey => apiKey.AllianceId == allianceId, cancellationToken);

        if (allianceApiKey is null) return Result.Failure<ApiKeyDto>(ApiKeyErrors.NoKeyForAlliance);

        return Result.Success(new ApiKeyDto()
        {
            Id = allianceApiKey.Id,
            CreatedBy = allianceApiKey.CreatedBy,
            CreatedOn = allianceApiKey.CreatedOn,
            AllianceId = allianceApiKey.AllianceId,
            ModifiedBy = allianceApiKey.ModifiedBy,
            ModifiedOn = allianceApiKey.ModifiedOn,
            Key = await encryptionService.Decrypt(allianceApiKey.EncryptedKey)
        });
    }

    public async Task<Result<ApiKeyDto>> CreateApiKeyAsync(CreateApiKeyDto createApiKeyDto, string creator, CancellationToken cancellationToken)
    {
        try
        {
            var apiKey = Guid.NewGuid().ToString("N");
            var newApiKey = new ApiKey()
            {
                Id = Guid.CreateVersion7(),
                CreatedBy = creator,
                EncryptedKey = await encryptionService.EncryptAsync(apiKey),
                AllianceId = createApiKeyDto.AllianceId,
                CreatedOn = DateTime.Now
            };

            await context.ApiKeys.AddAsync(newApiKey, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(new ApiKeyDto()
            {
                Id = newApiKey.Id,
                CreatedBy = newApiKey.CreatedBy,
                CreatedOn = newApiKey.CreatedOn,
                AllianceId = newApiKey.AllianceId,
                ModifiedBy = newApiKey.ModifiedBy,
                ModifiedOn = newApiKey.ModifiedOn,
                Key = apiKey
            });
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result.Failure<ApiKeyDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<ApiKeyDto>> UpdateApiKeyAsync(UpdateApiKeyDto updateApiKeyDto, string modifier, CancellationToken cancellationToken)
    {
        try
        {
            var apiKeyToUpdate = await context.ApiKeys
                .FirstOrDefaultAsync(apiKey => apiKey.Id == updateApiKeyDto.Id, cancellationToken);

            if (apiKeyToUpdate == null) return Result.Failure<ApiKeyDto>(ApiKeyErrors.NotFound);

            var newApiKey = Guid.NewGuid().ToString("N");

            apiKeyToUpdate.EncryptedKey = await encryptionService.EncryptAsync(newApiKey);
            apiKeyToUpdate.ModifiedBy = modifier;
            apiKeyToUpdate.ModifiedOn = DateTime.Now;

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(new ApiKeyDto()
            {
                Id = apiKeyToUpdate.Id,
                CreatedBy = apiKeyToUpdate.CreatedBy,
                CreatedOn = apiKeyToUpdate.CreatedOn,
                AllianceId = apiKeyToUpdate.AllianceId,
                ModifiedBy = apiKeyToUpdate.ModifiedBy,
                ModifiedOn = apiKeyToUpdate.ModifiedOn,
                Key = newApiKey
            });

        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result.Failure<ApiKeyDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<bool>> DeleteApiKeyAsync(Guid apiKeyId, CancellationToken cancellationToken)
    {
        try
        {
            var apiKeyToDelete = await context.ApiKeys
                .FirstOrDefaultAsync(apiKey => apiKey.Id == apiKeyId, cancellationToken);

            if (apiKeyToDelete == null) return Result.Failure<bool>(ApiKeyErrors.NotFound);

            context.ApiKeys.Remove(apiKeyToDelete);
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(true);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result.Failure<bool>(GeneralErrors.DatabaseError);
        }
    }
}