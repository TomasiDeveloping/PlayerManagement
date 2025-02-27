﻿using Application.Classes;
using Application.DataTransferObjects;
using Application.DataTransferObjects.ZombieSiege;

namespace Application.Interfaces;

public interface IZombieSiegeRepository
{
    Task<Result<ZombieSiegeDto>> GetZombieSiegeAsync(Guid zombieSiegeId, CancellationToken cancellationToken);

    Task<Result<ZombieSiegeDetailDto>> GetZombieSiegeDetailAsync(Guid zombieSiegeId, CancellationToken cancellationToken);

    Task<Result<PagedResponseDto<ZombieSiegeDto>>> GetAllianceZombieSiegesAsync(Guid allianceId, int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<Result<ZombieSiegeDto>> CreateZombieSiegeAsync(CreateZombieSiegeDto createZombieSiegeDto, string createdBy, CancellationToken cancellationToken);

    Task<Result<ZombieSiegeDto>> UpdateZombieSiegeAsync(UpdateZombieSiegeDto updateZombieSiegeDto, string modifiedBy, CancellationToken cancellationToken);

    Task<Result<bool>> DeleteZombieSiegeAsync(Guid zombieSiegeId, CancellationToken cancellationToken);
}