﻿using Application.Classes;
using Application.DataTransferObjects;
using Application.DataTransferObjects.CustomEvent;

namespace Application.Interfaces;

public interface ICustomEventRepository
{
    Task<Result<CustomEventDto>> GetCustomEventAsync(Guid customEventId, CancellationToken cancellationToken);

    Task<Result<CustomEventDetailDto>> GetCustomEventDetailAsync(Guid customEventId, CancellationToken cancellationToken);

    Task<Result<PagedResponseDto<CustomEventDto>>> GetAllianceCustomEventsAsync(Guid allianceId, int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<Result<CustomEventDto>> CreateCustomEventAsync(CreateCustomEventDto createCustomEventDto, string createdBy,
        CancellationToken cancellationToken);

    Task<Result<CustomEventDto>> UpdateCustomEventAsync(UpdateCustomEventDto updateCustomEventDto, string modifiedBy,
        CancellationToken cancellationToken);

    Task<Result<bool>> DeleteCustomEventAsync(Guid customEventId, CancellationToken cancellationToken);
}