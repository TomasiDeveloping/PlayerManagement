using Application.Classes;
using Application.DataTransferObjects.MarshalGuard;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Repositories;

public class MarshalGuardRepository(ApplicationContext context, IMapper mapper, ILogger<MarshalGuardRepository> logger) : IMarshalGuardRepository
{
    public async Task<Result<MarshalGuardDto>> GetMarshalGuardAsync(Guid marshalGuardId, CancellationToken cancellationToken)
    {
        var marshalGuardById = await context.MarshalGuards
            .ProjectTo<MarshalGuardDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(marshalGuard => marshalGuard.Id == marshalGuardId, cancellationToken);

        return marshalGuardById is null
            ? Result.Failure<MarshalGuardDto>(MarshalGuardErrors.NotFound)
            : Result.Success(marshalGuardById);
    }

    public async Task<Result<List<MarshalGuardDto>>> GetPlayerMarshalGuardsAsync(Guid playerId, CancellationToken cancellationToken)
    {
        var playerMarshalGuards = await context.MarshalGuards
            .Where(marshalGuard => marshalGuard.PlayerId == playerId)
            .ProjectTo<MarshalGuardDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success(playerMarshalGuards);
    }

    public async Task<Result<MarshalGuardDto>> CreateMarshalGuardAsync(CreateMarshalGuardDto createMarshalGuardDto, CancellationToken cancellationToken)
    {
        var newMarshalGuard = mapper.Map<MarshalGuard>(createMarshalGuardDto);

        await context.MarshalGuards.AddAsync(newMarshalGuard, cancellationToken);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<MarshalGuardDto>(newMarshalGuard));
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result.Failure<MarshalGuardDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<MarshalGuardDto>> UpdateMarshalGuardAsync(UpdateMarshalGuardDto updateMarshalGuardDto, CancellationToken cancellationToken)
    {
        var marshalGuardToUpdate = await context.MarshalGuards
            .FirstOrDefaultAsync(marshalGuard => marshalGuard.Id == updateMarshalGuardDto.Id, cancellationToken);

        if (marshalGuardToUpdate is null) return Result.Failure<MarshalGuardDto>(MarshalGuardErrors.NotFound);

        mapper.Map(updateMarshalGuardDto, marshalGuardToUpdate);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<MarshalGuardDto>(marshalGuardToUpdate));
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result.Failure<MarshalGuardDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<bool>> DeleteMarshalGuardAsync(Guid marshalGuardId, CancellationToken cancellationToken)
    {
        var marshalGuardToDelete = await context.MarshalGuards
            .FirstOrDefaultAsync(marshalGuard => marshalGuard.Id == marshalGuardId, cancellationToken);

        if (marshalGuardToDelete is null) return Result.Failure<bool>(MarshalGuardErrors.NotFound);

        context.MarshalGuards.Remove(marshalGuardToDelete);

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