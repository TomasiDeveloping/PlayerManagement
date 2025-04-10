using Application.Classes;
using Application.DataTransferObjects.MarshalGuardParticipant;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Repositories;

public class MarshalGuardParticipantRepository(ApplicationContext context, IMapper mapper, ILogger<MarshalGuardParticipantRepository> logger) : IMarshalGuardParticipantRepository
{
    public async Task<Result<MarshalGuardParticipantDto>> GetMarshalGuardParticipantAsync(Guid marshalGuardParticipantId, CancellationToken cancellationToken)
    {
        var marshalGuardParticipantById = await context.MarshalGuardParticipants
            .ProjectTo<MarshalGuardParticipantDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(marshalGuardParticipant => marshalGuardParticipant.Id == marshalGuardParticipantId,
                cancellationToken);
        return marshalGuardParticipantById is null
            ? Result.Failure<MarshalGuardParticipantDto>(new Error("", ""))
            : Result.Success(marshalGuardParticipantById);
    }

    public async Task<Result<bool>> InsertMarshalGuardParticipantAsync(List<CreateMarshalGuardParticipantDto> createMarshalGuardParticipantsDto,
        CancellationToken cancellationToken)
    {
        var newMarshalGuardParticipants = mapper.Map<List<MarshalGuardParticipant>>(createMarshalGuardParticipantsDto);

        await context.AddRangeAsync(newMarshalGuardParticipants, cancellationToken);

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

    public async Task<Result<List<MarshalGuardParticipantDto>>> GetPlayerMarshalParticipantsAsync(Guid playerId, int last, CancellationToken cancellationToken)
    {
        var playerMarshalParticipants = await context.MarshalGuardParticipants
            .Where(mp => mp.PlayerId == playerId)
            .OrderByDescending(mp => mp.MarshalGuard.EventDate)
            .Take(last)
            .ProjectTo<MarshalGuardParticipantDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success(playerMarshalParticipants);
    }

    public async Task<Result<MarshalGuardParticipantDto>> UpdateMarshalGuardParticipantAsync(UpdateMarshalGuardParticipantDto updateMarshalGuardParticipantDto,
        CancellationToken cancellationToken)
    {
        var participantToUpdate = await context.MarshalGuardParticipants
            .FirstOrDefaultAsync(
                marshalGuardParticipant => marshalGuardParticipant.Id == updateMarshalGuardParticipantDto.Id,
                cancellationToken);

        if (participantToUpdate is null) return Result.Failure<MarshalGuardParticipantDto>(MarshalGuardErrors.NotFound);

        mapper.Map(updateMarshalGuardParticipantDto, participantToUpdate);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<MarshalGuardParticipantDto>(participantToUpdate));
        }
        catch (Exception e)
        {
            logger.LogError(e, "{DateBaseErrorMessage}", e.Message);
            return Result.Failure<MarshalGuardParticipantDto>(GeneralErrors.DatabaseError);
        }
    }
}