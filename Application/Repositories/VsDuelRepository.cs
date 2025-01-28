using Application.Classes;
using Application.DataTransferObjects;
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

    public async Task<Result<VsDuelDetailDto>> GetVsDuelDetailAsync(Guid vsDuelId, CancellationToken cancellationToken)
    {
        var vsDuelDetail = await context.VsDuels
            .ProjectTo<VsDuelDetailDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(vsDuel => vsDuel.Id == vsDuelId, cancellationToken);

        return vsDuelDetail is null
            ? Result.Failure<VsDuelDetailDto>(VsDuelErrors.NotFound)
            : Result.Success(vsDuelDetail);
    }

    public async Task<Result<PagedResponseDto<VsDuelDto>>> GetAllianceVsDuelsAsync(Guid allianceId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = context.VsDuels
            .Where(vsDuel => vsDuel.AllianceId == allianceId)
            .OrderByDescending(vsDuel => vsDuel.EventDate)
            .AsNoTracking();

        var totalRecord = await query.CountAsync(cancellationToken);

        var pagedVsDuels = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<VsDuelDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result.Success(new PagedResponseDto<VsDuelDto>
        {
            Data = pagedVsDuels,
            TotalRecords = totalRecord,
            PageSize = pageSize,
            PageNumber = pageNumber
        });
    }

    public async Task<Result<VsDuelDto>> CreateVsDuelAsync(CreateVsDuelDto createVsDuelDto, string createdBy, CancellationToken cancellationToken)
    {
        var newVsDuel = mapper.Map<VsDuel>(createVsDuelDto);
        newVsDuel.CreatedBy = createdBy;

        await context.VsDuels.AddAsync(newVsDuel, cancellationToken);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            var vsDuelParticipantsResult =
                await InsertPlayersAsync(newVsDuel.Id, newVsDuel.AllianceId, cancellationToken);

            return vsDuelParticipantsResult.IsFailure
                ? Result.Failure<VsDuelDto>(vsDuelParticipantsResult.Error)
                : Result.Success(mapper.Map<VsDuelDto>(newVsDuel));
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result.Failure<VsDuelDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<VsDuelDto>> UpdateVsDuelAsync(UpdateVsDuelDto updateVsDuelDto, string modifiedBy, CancellationToken cancellationToken)
    {
        var vsDuelToUpdate = await context.VsDuels
            .FirstOrDefaultAsync(vsDuel => vsDuel.Id == updateVsDuelDto.Id, cancellationToken);

        if (vsDuelToUpdate is null) return Result.Failure<VsDuelDto>(VsDuelErrors.NotFound);

        mapper.Map(updateVsDuelDto, vsDuelToUpdate);
        vsDuelToUpdate.ModifiedBy = modifiedBy;

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

    private async Task<Result> InsertPlayersAsync(Guid vsDuelId, Guid allianceId, CancellationToken cancellationToken)
    {
        var alliancePlayers = await context.Players
            .Where(player => player.AllianceId == allianceId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var vsDuelParticipants = alliancePlayers.Select(player => new VsDuelParticipant() { Id = Guid.CreateVersion7(), PlayerId = player.Id, VsDuelId = vsDuelId, WeeklyPoints = 0 }).ToList();

        try
        {
            await context.VsDuelParticipants.AddRangeAsync(vsDuelParticipants, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(true);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result.Failure(GeneralErrors.DatabaseError);
        }
    }
}