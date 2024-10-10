using Application.Classes;
using Application.DataTransferObjects.VsDuel;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Repositories;

public class VsDuelRepository(ApplicationContext context, IMapper mapper, ILogger<VsDuelRepository> logger) : IVsDuelRepository
{
    public async Task<Result<VsDuelDto>> GetVsDuelAsync(Guid vsDuelId, CancellationToken cancellationToken)
    {
        var vsDuelById = await context.VsDuels
            .ProjectTo<VsDuelDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(vsDuel => vsDuel.Id == vsDuelId, cancellationToken);

        return vsDuelById is null
            ? Result.Failure<VsDuelDto>(VsDuelErrors.NotFound)
            : Result.Success(vsDuelById);
    }

    public async Task<Result<List<VsDuelDto>>> GetPlayerVsDuelsAsync(Guid playerId, CancellationToken cancellationToken)
    {
        var playerVsDuels = await context.VsDuels
            .Where(vsDuel => vsDuel.PlayerId == playerId)
            .ProjectTo<VsDuelDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success(playerVsDuels);
    }

    public async Task<Result<VsDuelDto>> CreateVsDuelAsync(CreateVsDuelDto createVsDuelDto, CancellationToken cancellationToken)
    {
        var newVsDuel = mapper.Map<VsDuel>(createVsDuelDto);

        await context.VsDuels.AddAsync(newVsDuel, cancellationToken);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<VsDuelDto>(newVsDuel));
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result.Failure<VsDuelDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<VsDuelDto>> UpdateVsDuelAsync(UpdateVsDuelDto updateVsDuelDto, CancellationToken cancellationToken)
    {
        var vsDuelToUpdate = await context.VsDuels
            .FirstOrDefaultAsync(vsDuel => vsDuel.Id == updateVsDuelDto.Id, cancellationToken);

        if (vsDuelToUpdate is null) return Result.Failure<VsDuelDto>(VsDuelErrors.NotFound);

        mapper.Map(updateVsDuelDto, vsDuelToUpdate);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<VsDuelDto>(vsDuelToUpdate));
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result.Failure<VsDuelDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<bool>> DeleteVsDuelAsync(Guid vsDuelId, CancellationToken cancellationToken)
    {
        var vsDuelToDelete = await context.VsDuels
            .FirstOrDefaultAsync(vsDuel => vsDuel.Id == vsDuelId, cancellationToken);

        if (vsDuelToDelete is null) return Result.Failure<bool>(VsDuelErrors.NotFound);

        context.VsDuels.Remove(vsDuelToDelete);

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