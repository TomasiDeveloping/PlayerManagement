using Application.Classes;
using Application.DataTransferObjects.Admonition;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Repositories;

public class AdmonitionRepository(ApplicationContext context, IMapper mapper, ILogger<AdmonitionRepository> logger) : IAdmonitionRepository
{
    public async Task<Result<List<AdmonitionDto>>> GetAdmonitionsAsync(CancellationToken cancellationToken)
    {
        var admonitions = await context.Admonitions
            .ProjectTo<AdmonitionDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success(admonitions);
    }

    public async Task<Result<List<AdmonitionDto>>> GetPlayerAdmonitionsAsync(Guid playerId, CancellationToken cancellationToken)
    {
        var playerAdmonitions = await context.Admonitions
            .Where(admonition => admonition.PlayerId == playerId)
            .ProjectTo<AdmonitionDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .OrderByDescending(admonition => admonition.CreatedOn)
            .ToListAsync(cancellationToken);

        return Result.Success(playerAdmonitions);
    }

    public async Task<Result<AdmonitionDto>> GetAdmonitionAsync(Guid admonitionId, CancellationToken cancellationToken)
    {
        var admonitionById = await context.Admonitions
            .ProjectTo<AdmonitionDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(admonition => admonition.Id == admonitionId, cancellationToken);

        return admonitionById is null
            ? Result.Failure<AdmonitionDto>(AdmonitionErrors.NotFound)
            : Result.Success(admonitionById);
    }

    public async Task<Result<AdmonitionDto>> CreateAdmonitionAsync(CreateAdmonitionDto createAdmonitionDto, string createdBy, CancellationToken cancellationToken)
    {
        var newAdmonition = mapper.Map<Admonition>(createAdmonitionDto);
        newAdmonition.CreatedBy = createdBy;

        await context.Admonitions.AddAsync(newAdmonition, cancellationToken);

        try
        {
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(mapper.Map<AdmonitionDto>(newAdmonition));
        }
        catch (Exception e)
        {
            logger.LogError(e, "{DateBaseErrorMessage}",e.Message);
            return Result.Failure<AdmonitionDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<AdmonitionDto>> UpdateAdmonitionAsync(UpdateAdmonitionDto updateAdmonitionDto, string modifiedBy, CancellationToken cancellationToken)
    {
        var admonitionToUpdate = await context.Admonitions
            .FirstOrDefaultAsync(admonition => admonition.Id == updateAdmonitionDto.Id, cancellationToken);

        if (admonitionToUpdate is null) return Result.Failure<AdmonitionDto>(AdmonitionErrors.NotFound);

        mapper.Map(updateAdmonitionDto, admonitionToUpdate);
        admonitionToUpdate.ModifiedBy = modifiedBy;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<AdmonitionDto>(admonitionToUpdate));
        }
        catch (Exception e)
        {
            logger.LogError(e, "{DateBaseErrorMessage}", e.Message);
            return Result.Failure<AdmonitionDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<bool>> DeleteAdmonitionAsync(Guid admonitionId, CancellationToken cancellationToken)
    {
        var admonitionToDelete = await context.Admonitions
            .FirstOrDefaultAsync(admonition => admonition.Id == admonitionId, cancellationToken);

        if (admonitionToDelete is null) return Result.Failure<bool>(AdmonitionErrors.NotFound);

        context.Admonitions.Remove(admonitionToDelete);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(true);
        }
        catch (Exception e)
        {
            logger.LogError(e, "{DateBaseErrorMessage}", e.Message);
            return Result.Failure<bool>(GeneralErrors.DatabaseError);
        }
    }
}