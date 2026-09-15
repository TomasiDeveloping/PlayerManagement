using Application.Errors;
using Application.Interfaces;
using Database;
using Database.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Repositories;

public class PlayerCombatRecordRepository(ApplicationContext dbContext, ILogger<PlayerCombatRecordRepository> logger) : IPlayerCombatRecordRepository
{
    public async Task AddAsync(PlayerCombatRecord record, CancellationToken cancellationToken = default)
    {
        await dbContext.PlayerCombatRecords.AddAsync(record, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "{DateBaseErrorMessage}", e.Message);
            throw new ApplicationException(GeneralErrors.DatabaseError.Name);
        }
    }
}