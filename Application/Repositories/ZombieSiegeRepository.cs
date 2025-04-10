using Application.Classes;
using Application.DataTransferObjects;
using Application.DataTransferObjects.ZombieSiege;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Repositories;

public class ZombieSiegeRepository(ApplicationContext context, IMapper mapper, ILogger<ZombieSiegeRepository> logger) : IZombieSiegeRepository
{
    public async Task<Result<ZombieSiegeDto>> GetZombieSiegeAsync(Guid zombieSiegeId, CancellationToken cancellationToken)
    {
        var zombieSiegeById = await context.ZombieSieges
            .ProjectTo<ZombieSiegeDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(zombieSiege => zombieSiege.Id == zombieSiegeId, cancellationToken);

        return zombieSiegeById is null
            ? Result.Failure<ZombieSiegeDto>(ZombieSiegeErrors.NotFound)
            : Result.Success(zombieSiegeById);
    }

    public async Task<Result<ZombieSiegeDetailDto>> GetZombieSiegeDetailAsync(Guid zombieSiegeId, CancellationToken cancellationToken)
    {
        var zombieSiegeDetail = await context.ZombieSieges
            .ProjectTo<ZombieSiegeDetailDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(zombieSiege => zombieSiege.Id == zombieSiegeId, cancellationToken);

        return zombieSiegeDetail is null
            ? Result.Failure<ZombieSiegeDetailDto>(ZombieSiegeErrors.NotFound)
            : Result.Success(zombieSiegeDetail);
    }

    public async Task<Result<PagedResponseDto<ZombieSiegeDto>>> GetAllianceZombieSiegesAsync(Guid allianceId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = context.ZombieSieges
            .Where(zombieSiege => zombieSiege.AllianceId == allianceId)
            .OrderByDescending(zombieSiege => zombieSiege.EventDate)
            .AsNoTracking();

        var totalRecord = await query.CountAsync(cancellationToken);

        var pagedZombieSieges = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<ZombieSiegeDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result.Success(new PagedResponseDto<ZombieSiegeDto>
        {
            Data = pagedZombieSieges,
            TotalRecords = totalRecord,
            PageSize = pageSize,
            PageNumber = pageNumber
        });
    }

    public async Task<Result<ZombieSiegeDto>> CreateZombieSiegeAsync(CreateZombieSiegeDto createZombieSiegeDto, string createdBy,
        CancellationToken cancellationToken)
    {
        var newZombieSiege = mapper.Map<ZombieSiege>(createZombieSiegeDto);
        newZombieSiege.CreatedBy = createdBy;

        try
        {
            await context.ZombieSieges.AddAsync(newZombieSiege, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(mapper.Map<ZombieSiegeDto>(newZombieSiege));
        }
        catch (Exception e)
        {
            logger.LogError(e, "{DateBaseErrorMessage}", e.Message);
            return Result.Failure<ZombieSiegeDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<ZombieSiegeDto>> UpdateZombieSiegeAsync(UpdateZombieSiegeDto updateZombieSiegeDto, string modifiedBy,
        CancellationToken cancellationToken)
    {
        var zombieSiegeToUpdate = await context.ZombieSieges
            .FirstOrDefaultAsync(zombieSiege => zombieSiege.Id == updateZombieSiegeDto.Id, cancellationToken);

        if (zombieSiegeToUpdate is null) return Result.Failure<ZombieSiegeDto>(ZombieSiegeErrors.NotFound);

        mapper.Map(updateZombieSiegeDto, zombieSiegeToUpdate);
        zombieSiegeToUpdate.ModifiedBy = modifiedBy;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<ZombieSiegeDto>(zombieSiegeToUpdate));
        }
        catch (Exception e)
        {
            logger.LogError(e, "{DateBaseErrorMessage}", e.Message);
            return Result.Failure<ZombieSiegeDto>(GeneralErrors.DatabaseError);
        }

    }

    public async Task<Result<bool>> DeleteZombieSiegeAsync(Guid zombieSiegeId, CancellationToken cancellationToken)
    {
        var zombieSiegeToDelete = await context.ZombieSieges
            .FirstOrDefaultAsync(zombieSiege => zombieSiege.Id == zombieSiegeId, cancellationToken);

        if (zombieSiegeToDelete is null) return Result.Failure<bool>(ZombieSiegeErrors.NotFound);

        try
        {
            context.ZombieSieges.Remove(zombieSiegeToDelete);
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