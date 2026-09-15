using Application.DataTransferObjects.PlayerCombatRecord;
using Application.Interfaces;
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
            Squad1 = dto.Squad1,
            Squad1Power = dto.Squad1Power,
            Squad2 = dto.Squad2,
            Squad2Power = dto.Squad2Power,
            Squad3 = dto.Squad3,
            Squad3Power = dto.Squad3Power,
            TotalHeroPower = dto.TotalHeroPower,
            Kills = dto.Kills,
            RecordedAtUtc = DateTime.UtcNow
        };
        await repository.AddAsync(record, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}