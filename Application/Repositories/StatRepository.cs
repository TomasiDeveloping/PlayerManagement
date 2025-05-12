using Application.Classes;
using Application.DataTransferObjects.Stat;
using Application.Interfaces;
using Database;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories;

public class StatRepository(ApplicationContext dbContext) : IStatRepository
{
    public async Task<Result<AllianceUseToolCount>> GetAllianceUseToolCountAsync(CancellationToken cancellationToken)
    {
        var allianceCount = await dbContext.Alliances.CountAsync(cancellationToken);

        return Result.Success(new AllianceUseToolCount
        {
            Amount = allianceCount
        });
    }
}