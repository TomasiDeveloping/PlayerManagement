using Application.Interfaces;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Repositories;

public class AllianceAccessTokenRepository(ApplicationContext dbContext, ILogger<AllianceAccessTokenRepository> logger) : IAllianceAccessTokenRepository
{
    public async Task<AllianceAccessToken?> GetActiveTokenByTokenStringAsync(string token, CancellationToken cancellationToken = default)
    {
        return await dbContext.AllianceAccessTokens
            .Include(t => t.Alliance)
            .FirstOrDefaultAsync(t => t.Token == token && t.IsActive, cancellationToken);
    }

    public async Task<AllianceAccessToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.AllianceAccessTokens
            .Include(t => t.Alliance)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<AllianceAccessToken>> GetTokensByAllianceIdAsync(Guid allianceId, CancellationToken cancellationToken = default)
    {
        return await dbContext.AllianceAccessTokens
            .Where(t => t.AllianceId == allianceId)
            .OrderByDescending(t => t.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(AllianceAccessToken accessToken, CancellationToken cancellationToken = default)
    {
        await dbContext.AllianceAccessTokens.AddAsync(accessToken, cancellationToken);
    }

    public Task UpdateAsync(AllianceAccessToken accessToken, CancellationToken cancellationToken = default)
    {
        dbContext.AllianceAccessTokens.Update(accessToken);
        return Task.CompletedTask;
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
            throw new Exception("An error occurred while saving changes to the database.", e);
        }
    }
}