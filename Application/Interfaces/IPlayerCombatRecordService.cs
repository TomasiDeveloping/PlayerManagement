using Application.DataTransferObjects.PlayerCombatRecord;

namespace Application.Interfaces;

public interface IPlayerCombatRecordService
{
    Task<bool> SubmitRecordAsync(SubmitPlayerCombatRecordDto dto, CancellationToken cancellationToken = default);

    Task<PlayerCombatDto?> GetPlayersByAllianceIdAsync(Guid allianceId, CancellationToken cancellationToken = default);

    Task<List<PlayerCombatRecordOverviewDto>> GetLatestRecordsByAllianceIdAsync(Guid allianceId, CancellationToken cancellationToken = default);

    Task<List<PlayerCombatRecordDto>> GetRecordsByPlayerIdAsync(Guid playerId, int? limit, CancellationToken cancellationToken = default);

    Task<PlayerCombatRecordDto> UpdateAsync(Guid id, UpdatePlayerCombatDto dto, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}