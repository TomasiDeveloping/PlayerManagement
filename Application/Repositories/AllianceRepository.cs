using Application.Classes;
using Application.DataTransferObjects.Alliance;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories;

public class AllianceRepository(ApplicationContext context, IMapper mapper) : IAllianceRepository
{
    public async Task<Result<List<AllianceDto>>> GetAlliancesAsync(CancellationToken cancellationToken)
    {
        var alliances = await context.Alliances
            .ProjectTo<AllianceDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success(alliances);
    }

    public async Task<Result<AllianceDto>> GetAllianceAsync(Guid allianceId, CancellationToken cancellationToken)
    {
        var allianceById = await context.Alliances
            .ProjectTo<AllianceDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(alliance => alliance.Id == allianceId, cancellationToken);

        return allianceById is null 
            ? Result.Failure<AllianceDto>(AllianceErrors.NotFound) 
            : Result.Success(allianceById);
    }

    public async Task<Result<AllianceDto>> CreateAllianceAsync(CreateAllianceDto createAllianceDto, CancellationToken cancellationToken)
    {
        var newAlliance = mapper.Map<Alliance>(createAllianceDto);

        await context.Alliances.AddAsync(newAlliance, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(mapper.Map<AllianceDto>(newAlliance));
    }

    public async Task<Result<AllianceDto>> UpdateAllianceAsync(UpdateAllianceDto updateAllianceDto, CancellationToken cancellationToken)
    {
        var allianceToUpdate = await context.Alliances
            .FirstOrDefaultAsync(alliance => alliance.Id == updateAllianceDto.Id, cancellationToken);

        if (allianceToUpdate is null) return Result.Failure<AllianceDto>(AllianceErrors.NotFound);

        mapper.Map(updateAllianceDto, allianceToUpdate);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(mapper.Map<AllianceDto>(allianceToUpdate));
    }

    public async Task<Result<bool>> DeleteAllianceAsync(Guid allianceId, CancellationToken cancellationToken)
    {
        var allianceToDelete =
            await context.Alliances.FirstOrDefaultAsync(alliance => alliance.Id == allianceId, cancellationToken);

        if (allianceToDelete is null) return Result.Failure<bool>(AllianceErrors.NotFound);

        context.Alliances.Remove(allianceToDelete);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(true);
    }
}