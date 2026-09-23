using Application.DataTransferObjects.PlayerCombatRecord;
using Application.Errors;
using Application.Interfaces;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
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

    public async Task<List<PlayerCombatRecord>> GetRecordsByPlayerIdAsync(Guid playerId,int? limit, CancellationToken cancellationToken = default)
    {
        var query = dbContext.PlayerCombatRecords
            .Where(r => r.PlayerId == playerId)
            .OrderByDescending(r => r.RecordedAtUtc)
            .AsNoTracking();

        if (limit.HasValue)
        {
            query = query.Take(limit.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<PlayerCombatRecordOverviewDto>> GetLatestRecordsByAllianceIdAsync(Guid allianceId, CancellationToken cancellationToken = default)
    {
        var players = await dbContext.Players
            .Where(p => p.AllianceId == allianceId)
            .Include(p => p.PlayerCombatRecords)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var result = players.Select(player =>
            {
                var latestRecord = player.PlayerCombatRecords?
                    .OrderByDescending(r => r.RecordedAtUtc)
                    .FirstOrDefault();

                return new PlayerCombatRecordOverviewDto
                {
                    PlayerId = player.Id,
                    PlayerName = player.PlayerName,
                    Squad1Power = latestRecord?.Squad1Power ?? 0,
                    Squad2Power = latestRecord?.Squad2Power ?? 0,
                    Squad3Power = latestRecord?.Squad3Power ?? 0,
                    TotalHeroPower = latestRecord?.TotalHeroPower ?? 0,
                    Kills = latestRecord?.Kills ?? 0,
                    Squad1 = latestRecord != null ? (int?)latestRecord.Squad1 : null,
                    Squad2 = latestRecord != null ? (int?)latestRecord.Squad2 : null,
                    Squad3 = latestRecord != null ? (int?)latestRecord.Squad3 : null,
                    RecordedAtUtc = latestRecord?.RecordedAtUtc
                };
            })
            .OrderByDescending(r => r.Squad1Power)
            .ToList();

        return result;
    }

    public async Task<PlayerCombatRecord> UpdateAsync(PlayerCombatRecord record, CancellationToken cancellationToken = default)
    {
        var recordToUpdate =
            await dbContext.PlayerCombatRecords.FirstOrDefaultAsync(r => r.Id == record.Id, cancellationToken);
         
        if (recordToUpdate is null) throw new ApplicationException("Record not found");

        recordToUpdate.Squad1 = record.Squad1;
        recordToUpdate.Squad1Power = record.Squad1Power;
        recordToUpdate.Squad2 = record.Squad2;
        recordToUpdate.Squad2Power = record.Squad2Power;
        recordToUpdate.Squad3 = record.Squad3;
        recordToUpdate.Squad3Power = record.Squad3Power;
        recordToUpdate.TotalHeroPower = record.TotalHeroPower;
        recordToUpdate.Kills = record.Kills;
        recordToUpdate.RecordedAtUtc = record.RecordedAtUtc;

        await dbContext.SaveChangesAsync(cancellationToken);
        return recordToUpdate;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var recordToDelete = await dbContext.PlayerCombatRecords.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        if (recordToDelete is null) return false;

        dbContext.PlayerCombatRecords.Remove(recordToDelete);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<PlayerCombatDto?> GetPlayersByAllianceIdAsync(Guid allianceId, CancellationToken cancellationToken = default)
    {
        var players = await dbContext.Players
            .Include(p => p.Alliance)
            .Where(r => r.AllianceId == allianceId)
            .ToListAsync(cancellationToken);

        if (players.Count == 0)
        {
            return null;
        }

        var dto = new PlayerCombatDto
        {
            AllianceName = players.First().Alliance.Name,
            Players = players.Select(p => new CombatRecordPlayer
            {
                Id = p.Id,
                Name = p.PlayerName
            }).ToList()
        };

        return dto;
    }
}