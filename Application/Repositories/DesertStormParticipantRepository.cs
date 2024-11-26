using Application.Classes;
using Application.DataTransferObjects.DesertStormParticipants;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Repositories;

public class DesertStormParticipantRepository(ApplicationContext context, ILogger<DesertStormParticipantRepository> logger, IMapper mapper) : IDesertStormParticipantRepository
{
    public async Task<Result<DesertStormParticipantDto>> GetDesertStormParticipantAsync(Guid desertStormParticipantId, CancellationToken cancellationToken)
    {
        var desertStormParticipantById = await context.DesertStormParticipants
            .ProjectTo<DesertStormParticipantDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(desertStormParticipant => desertStormParticipant.Id == desertStormParticipantId,
                cancellationToken);

        return desertStormParticipantById is null
            ? Result.Failure<DesertStormParticipantDto>(new Error("", ""))
            : Result.Success(desertStormParticipantById);
    }

    public async Task<Result<bool>> InsertDesertStormParticipantAsync(List<CreateDesertStormParticipantDto> createDesertStormParticipants, CancellationToken cancellationToken)
    {
        var desertStormParticipants = mapper.Map<List<DesertStormParticipant>>(createDesertStormParticipants);

        await context.DesertStormParticipants.AddRangeAsync(desertStormParticipants, cancellationToken);

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

    public async Task<Result<List<DesertStormParticipantDto>>> GetPlayerDesertStormParticipantsAsync(Guid playerId, int last, CancellationToken cancellationToken)
    {
        var desertStormPlayerParticipated = await context.DesertStormParticipants
            .Where(desertStormParticipant => desertStormParticipant.PlayerId == playerId)
            .OrderByDescending(desertStormParticipant => desertStormParticipant.DesertStorm.EventDate)
            .Take(last)
            .ProjectTo<DesertStormParticipantDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success(desertStormPlayerParticipated);
    }

    public async Task<Result<DesertStormParticipantDto>> UpdateDesertStormParticipantAsync(UpdateDesertStormParticipantDto updateDesertStormParticipantDto,
        CancellationToken cancellationToken)
    {
        var desertStormParticipantToUpdate = await context.DesertStormParticipants
            .FirstOrDefaultAsync(
                desertStormParticipant => desertStormParticipant.Id == updateDesertStormParticipantDto.Id,
                cancellationToken);

        if (desertStormParticipantToUpdate is null) return Result.Failure<DesertStormParticipantDto>(new Error("", ""));

        mapper.Map(updateDesertStormParticipantDto, desertStormParticipantToUpdate);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<DesertStormParticipantDto>(desertStormParticipantToUpdate));
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result.Failure<DesertStormParticipantDto>(GeneralErrors.DatabaseError);
        }
    }
}