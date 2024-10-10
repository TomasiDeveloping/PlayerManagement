using Application.Classes;
using Application.DataTransferObjects.DesertStorm;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Repositories;

public class DesertStormRepository(ApplicationContext context, IMapper mapper, ILogger<DesertStormRepository> logger) : IDesertStormRepository
{
    public async Task<Result<DesertStormDto>> GetDesertStormAsync(Guid desertStormId, CancellationToken cancellationToken)
    {
        var desertStormById = await context.DesertStorms
            .ProjectTo<DesertStormDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(desertStorm => desertStorm.Id == desertStormId, cancellationToken);

        return desertStormById is null
            ? Result.Failure<DesertStormDto>(DesertStormErrors.NotFound)
            : Result.Success(desertStormById);
    }

    public async Task<Result<List<DesertStormDto>>> GetPlayerDesertStormsAsync(Guid playerId, CancellationToken cancellationToken)
    {
        var playerDesertStorms = await context.DesertStorms
            .Where(desertStorm => desertStorm.PlayerId == playerId)
            .ProjectTo<DesertStormDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success(playerDesertStorms);
    }

    public async Task<Result<DesertStormDto>> CreateDesertStormAsync(CreateDesertStormDto createDesertStormDto, CancellationToken cancellationToken)
    {
        var newDesertStorm = mapper.Map<DesertStorm>(createDesertStormDto);

        await context.DesertStorms.AddAsync(newDesertStorm, cancellationToken);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<DesertStormDto>(newDesertStorm));
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result.Failure<DesertStormDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<DesertStormDto>> UpdateDesertStormAsync(UpdateDesertStormDto updateDesertStormDto, CancellationToken cancellationToken)
    {
        var desertStormToUpdate = await context.DesertStorms
            .FirstOrDefaultAsync(desertStorm => desertStorm.Id == updateDesertStormDto.Id, cancellationToken);

        if (desertStormToUpdate is null) return Result.Failure<DesertStormDto>(DesertStormErrors.NotFound);

        mapper.Map(updateDesertStormDto, desertStormToUpdate);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<DesertStormDto>(desertStormToUpdate));
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result.Failure<DesertStormDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<bool>> DeleteDesertStormAsync(Guid desertStormId, CancellationToken cancellationToken)
    {
        var desertStormToDelete = await context.DesertStorms
            .FirstOrDefaultAsync(desertStorm => desertStorm.Id == desertStormId, cancellationToken);

        if (desertStormToDelete is null) return Result.Failure<bool>(DesertStormErrors.NotFound);

        context.DesertStorms.Remove(desertStormToDelete);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(true);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result.Failure<bool>(GeneralErrors.DatabaseError);
        }
    }
}