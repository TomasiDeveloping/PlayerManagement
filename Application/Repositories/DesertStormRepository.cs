using Application.Classes;
using Application.DataTransferObjects;
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

    public async Task<Result<PagedResponseDto<DesertStormDto>>> GetAllianceDesertStormsAsync(Guid allianceId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = context.DesertStorms
            .Where(desertStorm => desertStorm.AllianceId == allianceId)
            .OrderByDescending(desertStorm => desertStorm.EventDate)
            .AsNoTracking();

        var totalRecord = await query.CountAsync(cancellationToken);

        var pagedDesertStorms = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<DesertStormDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result.Success(new PagedResponseDto<DesertStormDto>
        {
            Data = pagedDesertStorms,
            TotalRecords = totalRecord,
            PageSize = pageSize,
            PageNumber = pageNumber
        });
    }

    public async Task<Result<DesertStormDetailDto>> GetDesertStormDetailAsync(Guid desertStormId, CancellationToken cancellationToken)
    {
        var desertStormDetail = await context.DesertStorms
            .ProjectTo<DesertStormDetailDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(desertStorm => desertStorm.Id == desertStormId, cancellationToken);

        return desertStormDetail is null
            ? Result.Failure<DesertStormDetailDto>(DesertStormErrors.NotFound)
            : Result.Success(desertStormDetail);
    }

    public async Task<Result<DesertStormDto>> CreateDesertStormAsync(CreateDesertStormDto createDesertStormDto, string createdBy, CancellationToken cancellationToken)
    {
        var newDesertStorm = mapper.Map<DesertStorm>(createDesertStormDto);
        newDesertStorm.CreatedBy = createdBy;

        await context.DesertStorms.AddAsync(newDesertStorm, cancellationToken);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<DesertStormDto>(newDesertStorm));
        }
        catch (Exception e)
        {
            logger.LogError(e, "{DateBaseErrorMessage}", e.Message);
            return Result.Failure<DesertStormDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<DesertStormDto>> UpdateDesertStormAsync(UpdateDesertStormDto updateDesertStormDto, string modifiedBy, CancellationToken cancellationToken)
    {
        var desertStormToUpdate = await context.DesertStorms
            .FirstOrDefaultAsync(desertStorm => desertStorm.Id == updateDesertStormDto.Id, cancellationToken);

        if (desertStormToUpdate is null) return Result.Failure<DesertStormDto>(DesertStormErrors.NotFound);

        mapper.Map(updateDesertStormDto, desertStormToUpdate);
        desertStormToUpdate.ModifiedBy = modifiedBy;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<DesertStormDto>(desertStormToUpdate));
        }
        catch (Exception e)
        {
            logger.LogError(e, "{DateBaseErrorMessage}", e.Message);
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
            logger.LogError(e, "{DateBaseErrorMessage}", e.Message);
            return Result.Failure<bool>(GeneralErrors.DatabaseError);
        }
    }
}