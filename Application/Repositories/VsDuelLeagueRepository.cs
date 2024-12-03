using Application.Classes;
using Application.DataTransferObjects.VsDuelLeague;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories;

public class VsDuelLeagueRepository(ApplicationContext context, IMapper mapper) : IVsDuelLeagueRepository
{
    public async Task<Result<List<VsDuelLeagueDto>>> GetVsDuelLeaguesAsync(CancellationToken cancellationToken)
    {
        var vsDuelLeagues = await context.VsDuelLeagues
            .ProjectTo<VsDuelLeagueDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .OrderBy(vsDuelLeague => vsDuelLeague.Code)
            .ToListAsync(cancellationToken);

        return Result.Success(vsDuelLeagues);
    }
}