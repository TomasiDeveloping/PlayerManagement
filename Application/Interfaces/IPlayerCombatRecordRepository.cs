using Application.DataTransferObjects.PlayerCombatRecord;
using Database.Entities;

namespace Application.Interfaces;

public interface IPlayerCombatRecordRepository
{
    Task AddAsync(PlayerCombatRecord record, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<List<PlayerCombatRecord>> GetRecordsByPlayerIdAsync(Guid playerId,int? limit, CancellationToken cancellationToken = default);

    Task<List<PlayerCombatRecordOverviewDto>> GetLatestRecordsByAllianceIdAsync(Guid allianceId,
        CancellationToken cancellationToken = default);

    Task<PlayerCombatRecord> UpdateAsync(PlayerCombatRecord record, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<PlayerCombatDto?> GetPlayersByAllianceIdAsync(Guid allianceId, CancellationToken cancellationToken = default);
}