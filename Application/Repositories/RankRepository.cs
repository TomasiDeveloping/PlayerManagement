using Application.Classes;
using Application.DataTransferObjects.Rank;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories;

public class RankRepository(ApplicationContext context, IMapper mapper) : IRankRepository
{
    public async Task<Result<List<RankDto>>> GetRanksAsync(CancellationToken cancellationToken)
    {
        var ranks = await context.Ranks
            .ProjectTo<RankDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .OrderByDescending(rank => rank.Name)
            .ToListAsync(cancellationToken);

        return Result.Success(ranks);
    }
}