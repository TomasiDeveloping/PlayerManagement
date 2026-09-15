using Application.DataTransferObjects.AllianceAccessToken;

namespace Application.Interfaces;

public interface IAllianceAccessTokenService
{
    Task<AllianceAccessTokenDto> GenerateNewTokenAsync(CreateAllianceAccessTokenDto dto, string baseUrl, CancellationToken cancellationToken = default);
    Task<bool> RevokeTokenAsync(Guid tokenId, CancellationToken cancellationToken = default);
    Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<IEnumerable<AllianceAccessTokenDto>> GetTokensForAllianceAsync(Guid allianceId, string baseUrl, CancellationToken cancellationToken = default);
}