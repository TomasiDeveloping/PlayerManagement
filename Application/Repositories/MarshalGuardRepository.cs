using Application.Classes;
using Application.DataTransferObjects;
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

    public async Task<Result<MarshalGuardDetailDto>> GetMarshalGuardDetailAsync(Guid marshalGuardId, CancellationToken cancellationToken)
    {
        var detailMarshalGuard = await context.MarshalGuards
            .ProjectTo<MarshalGuardDetailDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(marshalGuard => marshalGuard.Id == marshalGuardId, cancellationToken);

        return detailMarshalGuard is null
            ? Result.Failure<MarshalGuardDetailDto>(MarshalGuardErrors.NotFound)
            : Result.Success(detailMarshalGuard);
    }

    public async Task<Result<PagedResponseDto<MarshalGuardDto>>> GetAllianceMarshalGuardsAsync(Guid allianceId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = context.MarshalGuards
            .Where(marshalGuard => marshalGuard.AllianceId == allianceId)
            .OrderByDescending(marshalGuard => marshalGuard.EventDate)
            .AsNoTracking();

        var totalRecord = await query.CountAsync(cancellationToken);

        var pagedMarshalGuards = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<MarshalGuardDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result.Success(new PagedResponseDto<MarshalGuardDto>
        {
            Data = pagedMarshalGuards,
            TotalRecords = totalRecord,
            PageSize = pageSize,
            PageNumber = pageNumber
        });
    }


    public async Task<Result<MarshalGuardDto>> CreateMarshalGuardsAsync(CreateMarshalGuardDto createMarshalGuardDto, string createdBy, CancellationToken cancellationToken)
    {
        var newMarshalGuard = mapper.Map<MarshalGuard>(createMarshalGuardDto);
        newMarshalGuard.CreatedBy = createdBy;

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

    public async Task<Result<MarshalGuardDto>> UpdateMarshalGuardAsync(UpdateMarshalGuardDto updateMarshalGuardDto, string modifiedBy, CancellationToken cancellationToken)
    {
        var marshalGuardToUpdate = await context.MarshalGuards
            .FirstOrDefaultAsync(marshalGuard => marshalGuard.Id == updateMarshalGuardDto.Id, cancellationToken);

        if (marshalGuardToUpdate is null) return Result.Failure<MarshalGuardDto>(MarshalGuardErrors.NotFound);

        mapper.Map(updateMarshalGuardDto, marshalGuardToUpdate);
        marshalGuardToUpdate.ModifiedBy = modifiedBy;

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