using Database.Entities;

namespace Application.Interfaces;

public interface IAllianceAccessTokenRepository
{
    Task<AllianceAccessToken?> GetActiveTokenByTokenStringAsync(string token, CancellationToken cancellationToken = default);
    Task<AllianceAccessToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AllianceAccessToken>> GetTokensByAllianceIdAsync(Guid allianceId, CancellationToken cancellationToken = default);
    Task AddAsync(AllianceAccessToken accessToken, CancellationToken cancellationToken = default);
    Task UpdateAsync(AllianceAccessToken accessToken, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}