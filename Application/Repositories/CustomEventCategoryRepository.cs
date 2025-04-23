using Application.Classes;
using Application.DataTransferObjects.CustomEventCategory;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Repositories;

public class CustomEventCategoryRepository(ApplicationContext dbContext, IMapper mapper, ILogger<CustomEventCategoryRepository> logger) : ICustomEventCategoryRepository
{
    public async Task<Result<CustomEventCategoryDto>> GetCustomEventCategoryAsync(Guid customEventCategoryId, CancellationToken cancellationToken)
    {
        var customEventCategoryById = await dbContext.CustomEventCategories
            .ProjectTo<CustomEventCategoryDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(customEventCategory => customEventCategory.Id == customEventCategoryId, cancellationToken);

        return customEventCategoryById is null
            ? Result.Failure<CustomEventCategoryDto>(CustomEventCategoryErrors.NotFound)
            : Result.Success(customEventCategoryById);
    }

    public async Task<Result<List<CustomEventCategoryDto>>> GetAllianceCustomEventCategoriesAsync(Guid allianceId, CancellationToken cancellationToken)
    {

        var customEventCategories = await dbContext.CustomEventCategories
                .Where(customEventCategory => customEventCategory.AllianceId == allianceId)
                .ProjectTo<CustomEventCategoryDto>(mapper.ConfigurationProvider)
                .OrderByDescending(customEventCategory => customEventCategory.Id)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

        return Result.Success(customEventCategories);
    }

    public async Task<Result<CustomEventCategoryDto>> CreateCustomEventCategoryAsync(CreateCustomEventCategoryDto createCustomEventCategoryDto,
        CancellationToken cancellationToken)
    {
        try
        {
            var newCustomEventCategory = mapper.Map<CustomEventCategory>(createCustomEventCategoryDto);
            await dbContext.CustomEventCategories.AddAsync(newCustomEventCategory, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            var customEventCategoryDto = mapper.Map<CustomEventCategoryDto>(newCustomEventCategory);
            return Result.Success(customEventCategoryDto);
        }
        catch (Exception e)
        {
            logger.LogError(e, "{ErrorMessage}", e.Message);
            return Result.Failure<CustomEventCategoryDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<CustomEventCategoryDto>> UpdateCustomEventCategoryAsync(UpdateCustomEventCategoryDto updateCustomEventCategoryDto,
        CancellationToken cancellationToken)
    {
        var customEventCategoryToUpdate = await dbContext.CustomEventCategories
            .FirstOrDefaultAsync(customEventCategory => customEventCategory.Id == updateCustomEventCategoryDto.Id, cancellationToken);
        if (customEventCategoryToUpdate is null)
        {
            return Result.Failure<CustomEventCategoryDto>(CustomEventCategoryErrors.NotFound);
        }

        try
        {
            mapper.Map(updateCustomEventCategoryDto, customEventCategoryToUpdate);
            await dbContext.SaveChangesAsync(cancellationToken);
            var customEventCategoryDto = mapper.Map<CustomEventCategoryDto>(customEventCategoryToUpdate);
            return Result.Success(customEventCategoryDto);
        }
        catch (Exception e)
        {
            logger.LogError(e, "{ErrorMessage}", e.Message);
            return Result.Failure<CustomEventCategoryDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<bool>> DeleteCustomEventAsync(Guid customEventId, CancellationToken cancellationToken)
    {
        var customEventToDelete = await dbContext.CustomEventCategories
            .FirstOrDefaultAsync(customEvent => customEvent.Id == customEventId, cancellationToken);
        if (customEventToDelete is null)
        {
            return Result.Failure<bool>(CustomEventCategoryErrors.NotFound);
        }

        try
        {
            var customEvents = await dbContext.CustomEvents
                .Where(customEvent => customEvent.CustomEventCategoryId == customEventToDelete.Id)
                .ToListAsync(cancellationToken);
            if (customEvents.Any())
            {
                dbContext.CustomEvents.RemoveRange(customEvents);
            }
            dbContext.CustomEventCategories.Remove(customEventToDelete);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success(true);
        }
        catch (Exception e)
        {
            logger.LogError(e, "{ErrorMessage}", e.Message);
            return Result.Failure<bool>(GeneralErrors.DatabaseError);
        }
    }
}