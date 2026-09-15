using Application.DataTransferObjects.PlayerCombatRecord;

namespace Application.Interfaces;

public interface IPlayerCombatRecordService
{
    Task<bool> SubmitRecordAsync(SubmitPlayerCombatRecordDto dto, CancellationToken cancellationToken = default);
}