using Application.DataTransferObjects.PlayerCombatRecord;
using Application.Interfaces;
using Database;
using Database.Entities;

namespace Application.Services;

public class PlayerCombatRecordService(IPlayerCombatRecordRepository repository) : IPlayerCombatRecordService
{
    public async Task<bool> SubmitRecordAsync(SubmitPlayerCombatRecordDto dto, CancellationToken cancellationToken = default)
    {
        var record = new PlayerCombatRecord
        {
            Id = Guid.CreateVersion7(),
            PlayerId = dto.PlayerId,
            Squad1 = Enum.Parse<TroopType>(dto.Squad1, true),
            Squad1Power = dto.Squad1Power,
            Squad2 = Enum.Parse<TroopType>(dto.Squad2, true),
            Squad2Power = dto.Squad2Power,
            Squad3 = Enum.Parse<TroopType>(dto.Squad3, true),
            Squad3Power = dto.Squad3Power,
            TotalHeroPower = dto.TotalHeroPower,
            Kills = dto.Kills,
            RecordedAtUtc = dto.RecordedAtUtc ?? DateTime.UtcNow
        };
        await repository.AddAsync(record, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<PlayerCombatDto?> GetPlayersByAllianceIdAsync(Guid allianceId, CancellationToken cancellationToken = default)
    {
        var dto = await repository.GetPlayersByAllianceIdAsync(allianceId, cancellationToken);
        return dto;
    }

    public async Task<List<PlayerCombatRecordDto>> GetRecordsByPlayerIdAsync(Guid playerId,int? limit, CancellationToken cancellationToken = default)
    {
        var records = await repository.GetRecordsByPlayerIdAsync(playerId, limit, cancellationToken);
        return records.Select(r => new PlayerCombatRecordDto
        {
            Id = r.Id,
            PlayerId = r.PlayerId,
            Squad1 = r.Squad1,
            Squad1Power = r.Squad1Power,
            Squad2 = r.Squad2,
            Squad2Power = r.Squad2Power,
            Squad3 = r.Squad3,
            Squad3Power = r.Squad3Power,
            TotalHeroPower = r.TotalHeroPower,
            Kills = r.Kills,
            RecordedAtUtc = r.RecordedAtUtc
        }).ToList();
    }

    public async Task<PlayerCombatRecordDto> UpdateAsync(Guid id, UpdatePlayerCombatDto dto, CancellationToken cancellationToken = default)
    {
        var record = new PlayerCombatRecord
        {
            Id = id,
            PlayerId = dto.PlayerId,
            Squad1 = Enum.Parse<TroopType>(dto.Squad1, true),
            Squad1Power = dto.Squad1Power,
            Squad2 = Enum.Parse<TroopType>(dto.Squad2, true),
            Squad2Power = dto.Squad2Power,
            Squad3 = Enum.Parse<TroopType>(dto.Squad3, true),
            Squad3Power = dto.Squad3Power,
            TotalHeroPower = dto.TotalHeroPower,
            Kills = dto.Kills,
            RecordedAtUtc = dto.RecordedAtUtc
        };
        var updatedRecord = await repository.UpdateAsync(record, cancellationToken);
        return new PlayerCombatRecordDto
        {
            Id = updatedRecord.Id,
            PlayerId = updatedRecord.PlayerId,
            Squad1 = updatedRecord.Squad1,
            Squad1Power = updatedRecord.Squad1Power,
            Squad2 = updatedRecord.Squad2,
            Squad2Power = updatedRecord.Squad2Power,
            Squad3 = updatedRecord.Squad3,
            Squad3Power = updatedRecord.Squad3Power,
            TotalHeroPower = updatedRecord.TotalHeroPower,
            Kills = updatedRecord.Kills,
            RecordedAtUtc = updatedRecord.RecordedAtUtc
        };
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.DeleteAsync(id, cancellationToken);
        return result;
    }

    public async Task<List<PlayerCombatRecordOverviewDto>> GetLatestRecordsByAllianceIdAsync(Guid allianceId, CancellationToken cancellationToken = default)
    {
        var records = await repository.GetLatestRecordsByAllianceIdAsync(allianceId, cancellationToken);
        return records;
    }
}