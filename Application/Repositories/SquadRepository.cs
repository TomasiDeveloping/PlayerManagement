using Application.Classes;
using Application.DataTransferObjects.Squad;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Repositories;

public class SquadRepository(ApplicationContext dbContext, IMapper mapper, ILogger<SquadRepository> logger) : ISquadRepository
{
    public async Task<Result<List<SquadDto>>> GetPlayerSquadsAsync(Guid playerId, CancellationToken cancellationToken = default)
    {
        var playerSquads = await dbContext.Squads
            .Where(squad => squad.PlayerId == playerId)
            .ProjectTo<SquadDto>(mapper.ConfigurationProvider)
            .OrderBy(squad => squad.Position)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success(playerSquads);
    }

    public async Task<Result<SquadDto>> CreateSquadAsync(CreateSquadDto createSquadDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var newSuad = mapper.Map<Squad>(createSquadDto);

            await dbContext.Squads.AddAsync(newSuad, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(mapper.Map<SquadDto>(newSuad));
        }
        catch (Exception e)
        {
            logger.LogError(e, "{DatabaseException}", e.Message);
            return Result.Failure<SquadDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<SquadDto>> UpdateSquadAsync(UpdateSquadDto updateSquadDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var squadToUpdate = await dbContext.Squads
                .FirstOrDefaultAsync(squad => squad.Id == updateSquadDto.Id, cancellationToken);

            if (squadToUpdate is null) return Result.Failure<SquadDto>(SquadErrors.NotFound);

            mapper.Map(updateSquadDto, squadToUpdate);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(mapper.Map<SquadDto>(squadToUpdate));
        }
        catch (Exception e)
        {
            logger.LogError(e, "{DatabaseException}", e.Message);
            return Result.Failure<SquadDto>(GeneralErrors.DatabaseError);
        }

    }

    public async Task<Result<bool>> DeleteSquadAsync(Guid squadId, CancellationToken cancellationToken = default)
    {
        try
        {
            var squadToDelete = await dbContext.Squads
                .FirstOrDefaultAsync(squad => squad.Id == squadId, cancellationToken);

            if (squadToDelete is null) return Result.Failure<bool>(SquadErrors.NotFound);

            dbContext.Squads.Remove(squadToDelete);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(mapper.Map<bool>(true));
        }
        catch (Exception e)
        {
            logger.LogError(e, "{DatabaseException}", e.Message);
            return Result.Failure<bool>(GeneralErrors.DatabaseError);
        }
    }
}