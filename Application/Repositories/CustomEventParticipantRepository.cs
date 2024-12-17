using Application.Classes;
using Application.DataTransferObjects.CustomEventParticipant;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Repositories;

public class CustomEventParticipantRepository(ApplicationContext context, IMapper mapper, ILogger<CustomEventParticipantRepository> logger) : ICustomEventParticipantRepository
{
    public async Task<Result<CustomEventParticipantDto>> GetCustomEventParticipantAsync(Guid customEventParticipantId, CancellationToken cancellationToken)
    {
        var customEventParticipantById = await context.CustomEventParticipants
            .ProjectTo<CustomEventParticipantDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(customEventParticipant => customEventParticipant.Id == customEventParticipantId,
                cancellationToken);

        return customEventParticipantById is null
            ? Result.Failure<CustomEventParticipantDto>(new Error("", ""))
            : Result.Success(customEventParticipantById);
    }

    public async Task<Result<bool>> InsertCustomEventParticipantAsync(List<CreateCustomEventParticipantDto> createCustomEventParticipants, CancellationToken cancellationToken)
    {
        var customEventParticipants = mapper.Map<List<CustomEventParticipant>>(createCustomEventParticipants);

        await context.CustomEventParticipants.AddRangeAsync(customEventParticipants, cancellationToken);

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

    public async Task<Result<List<CustomEventParticipantDto>>> GetPlayerCustomEventParticipantsAsync(Guid playerId, int last, CancellationToken cancellationToken)
    {
        var customEventPlayerParticipated = await context.CustomEventParticipants
            .Where(customEventParticipant => customEventParticipant.PlayerId == playerId)
            .OrderByDescending(customEventParticipant => customEventParticipant.CustomEvent.EventDate)
            .Take(last)
            .ProjectTo<CustomEventParticipantDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success(customEventPlayerParticipated);
    }

    public async Task<Result<CustomEventParticipantDto>> UpdateCustomEventParticipantAsync(UpdateCustomEventParticipantDto updateCustomEventParticipant,
        CancellationToken cancellationToken)
    {
        var customEventParticipantToUpdate = await context.CustomEventParticipants
            .FirstOrDefaultAsync(
                customEventParticipant => customEventParticipant.Id == updateCustomEventParticipant.Id,
                cancellationToken);

        if (customEventParticipantToUpdate is null) return Result.Failure<CustomEventParticipantDto>(new Error("", ""));

        mapper.Map(updateCustomEventParticipant, customEventParticipantToUpdate);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<CustomEventParticipantDto>(customEventParticipantToUpdate));
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result.Failure<CustomEventParticipantDto>(GeneralErrors.DatabaseError);
        }
    }
}