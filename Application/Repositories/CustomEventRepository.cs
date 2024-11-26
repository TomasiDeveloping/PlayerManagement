using Application.Classes;
using Application.DataTransferObjects.CustomEvent;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Repositories;

public class CustomEventRepository(ApplicationContext context, IMapper mapper, ILogger<CustomEventRepository> logger) : ICustomEventRepository
{
    public async Task<Result<CustomEventDto>> GetCustomEventAsync(Guid customEventId, CancellationToken cancellationToken)
    {
        var customEventById = await context.CustomEvents
            .ProjectTo<CustomEventDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(customEvent => customEvent.Id == customEventId, cancellationToken);

        return customEventById is null
            ? Result.Failure<CustomEventDto>(CustomEventErrors.NotFound)
            : Result.Success(customEventById);
    }

    public async Task<Result<CustomEventDetailDto>> GetCustomEventDetailAsync(Guid customEventId, CancellationToken cancellationToken)
    {
        var customEventDetail = await context.CustomEvents
            .ProjectTo<CustomEventDetailDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(customEvent => customEvent.Id == customEventId, cancellationToken);

        return customEventDetail is null
            ? Result.Failure<CustomEventDetailDto>(CustomEventErrors.NotFound)
            : Result.Success(customEventDetail);
    }

    public async Task<Result<List<CustomEventDto>>> GetAllianceCustomEventsAsync(Guid allianceId, int take, CancellationToken cancellationToken)
    {
        var allianceCustomEvents = await context.CustomEvents
            .Where(customEvent => customEvent.AllianceId == allianceId)
            .ProjectTo<CustomEventDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .OrderByDescending(customEvent => customEvent.EventDate)
            .Take(take)
            .ToListAsync(cancellationToken);

        return Result.Success(allianceCustomEvents);
    }

    public async Task<Result<CustomEventDto>> CreateCustomEventAsync(CreateCustomEventDto createCustomEventDto, string createdBy,
        CancellationToken cancellationToken)
    {
        var newCustomEvent = mapper.Map<CustomEvent>(createCustomEventDto);

        newCustomEvent.CreatedBy = createdBy;

        await context.CustomEvents.AddAsync(newCustomEvent, cancellationToken);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<CustomEventDto>(newCustomEvent));
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result.Failure<CustomEventDto>(GeneralErrors.DatabaseError);
        }

    }

    public async Task<Result<CustomEventDto>> UpdateCustomEventAsync(UpdateCustomEventDto updateCustomEventDto, string modifiedBy,
        CancellationToken cancellationToken)
    {
        var customEventToUpdate =
            await context.CustomEvents.FirstOrDefaultAsync(customEvent => customEvent.Id == updateCustomEventDto.Id,
                cancellationToken);

        if (customEventToUpdate is null) return Result.Failure<CustomEventDto>(CustomEventErrors.NotFound);

        mapper.Map(updateCustomEventDto, customEventToUpdate);
        customEventToUpdate.ModifiedBy = modifiedBy;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<CustomEventDto>(customEventToUpdate));
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result.Failure<CustomEventDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<bool>> DeleteCustomEventAsync(Guid customEventId, CancellationToken cancellationToken)
    {
        var customEventToDelete =
            await context.CustomEvents.FirstOrDefaultAsync(customEvent => customEvent.Id == customEventId, cancellationToken);

        if (customEventToDelete is null) return Result.Failure<bool>(CustomEventErrors.NotFound);

        context.CustomEvents.Remove(customEventToDelete);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<bool>(true));
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result.Failure<bool>(GeneralErrors.DatabaseError);
        }
    }
}