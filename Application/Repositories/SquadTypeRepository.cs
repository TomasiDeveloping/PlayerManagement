using Application.Classes;
using Application.DataTransferObjects.SquadType;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories;

public class SquadTypeRepository(ApplicationContext dbContext, IMapper mapper) : ISquadTypeRepository
{
    public async Task<Result<List<SquadTypeDto>>> GetSquadTypesAsync(CancellationToken cancellationToken)
    {
        var squadTypes = await dbContext.SquadTypes
            .ProjectTo<SquadTypeDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success(squadTypes);
    }
}