using Database.Entities;

namespace Application.Interfaces;

public interface IPlayerCombatRecordRepository
{
    Task AddAsync(PlayerCombatRecord record, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}