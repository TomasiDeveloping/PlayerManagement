using Application.Classes;
using Application.DataTransferObjects.CustomEventCategory;

namespace Application.Interfaces;

public interface ICustomEventCategoryRepository
{
    Task<Result<CustomEventCategoryDto>> GetCustomEventCategoryAsync(Guid customEventCategoryId, CancellationToken cancellationToken);

    Task<Result<List<CustomEventCategoryDto>>> GetAllianceCustomEventCategoriesAsync(Guid allianceId, CancellationToken cancellationToken);

    Task<Result<CustomEventCategoryDto>> CreateCustomEventCategoryAsync(CreateCustomEventCategoryDto createCustomEventCategoryDto, CancellationToken cancellationToken);

    Task<Result<CustomEventCategoryDto>> UpdateCustomEventCategoryAsync(UpdateCustomEventCategoryDto updateCustomEventCategoryDto, CancellationToken cancellationToken);

    Task<Result<bool>> DeleteCustomEventAsync(Guid customEventId, CancellationToken cancellationToken);
}