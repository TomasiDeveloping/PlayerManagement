using Application.DataTransferObjects.AllianceAccessToken;
using Application.Interfaces;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class AllianceAccessTokenService(IAllianceAccessTokenRepository repository) : IAllianceAccessTokenService
{
    public async Task<AllianceAccessTokenDto> GenerateNewTokenAsync(CreateAllianceAccessTokenDto dto, string baseUrl, CancellationToken cancellationToken)
    {
        var existingTokens = await repository.GetTokensByAllianceIdAsync(dto.AllianceId, cancellationToken);
        foreach (var token in existingTokens.Where(x => x.IsActive))
        {
            token.IsActive = false;
            await repository.UpdateAsync(token, cancellationToken);
        }

        var uniqueTokenString = GenerateSecureTokenString();

        var newToken = new AllianceAccessToken
        {
            Id = Guid.CreateVersion7(),
            AllianceId = dto.AllianceId,
            Token = uniqueTokenString,
            ExpiresAtUtc = dto.ExpiresAt,
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        await repository.AddAsync(newToken, cancellationToken);

        try
        {
            await repository.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            newToken.Token = GenerateSecureTokenString();
            await repository.SaveChangesAsync(cancellationToken);
        }

        return MapToDto(newToken, baseUrl);
    }

    public async Task<bool> RevokeTokenAsync(Guid tokenId, CancellationToken cancellationToken = default)
    {
        var token = await repository.GetByIdAsync(tokenId, cancellationToken);
        if (token is null) return false;

        token.IsActive = false;
        await repository.UpdateAsync(token, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<AllianceAccessTokenDto?> ValidateTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var accessToken = await repository.GetActiveTokenByTokenStringAsync(token, cancellationToken);

        if (accessToken == null) return null;

        if (accessToken.ExpiresAtUtc.HasValue && accessToken.ExpiresAtUtc.Value < DateTime.UtcNow)
        {
            return null;
        }

        return new AllianceAccessTokenDto()
        {
            Token = accessToken.Token,
            IsActive = accessToken.IsActive,
            AllianceId = accessToken.AllianceId,
            CreatedAt = accessToken.CreatedAtUtc,
            ExpiresAt = accessToken.ExpiresAtUtc,
            FullShareUrl = "",
            Id = accessToken.Id
        };
    }

    public async Task<IEnumerable<AllianceAccessTokenDto>> GetTokensForAllianceAsync(Guid allianceId, string baseUrl, CancellationToken cancellationToken = default)
    {
        var tokens = await repository.GetTokensByAllianceIdAsync(allianceId, cancellationToken);
        return tokens.Select(token => MapToDto(token, baseUrl));
    }

    private static string GenerateSecureTokenString()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace("/", "_")
            .Replace("+", "-")
            .TrimEnd('=');
    }

    private static AllianceAccessTokenDto MapToDto(AllianceAccessToken entity, string baseUrl)
    {
        var cleanBaseUrl = baseUrl.TrimEnd('/');
        return new AllianceAccessTokenDto
        {
            Id = entity.Id,
            AllianceId = entity.AllianceId,
            Token = entity.Token,
            FullShareUrl = $"{cleanBaseUrl}/combat-power?token={entity.Token}",
            ExpiresAt = entity.ExpiresAtUtc,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAtUtc
        };
    }
}