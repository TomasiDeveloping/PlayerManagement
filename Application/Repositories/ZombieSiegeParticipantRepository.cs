using Application.Classes;
using Application.DataTransferObjects.ZombieSiegeParticipant;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Repositories;

public class ZombieSiegeParticipantRepository(ApplicationContext context, IMapper mapper, ILogger<ZombieSiegeParticipantRepository> logger) : IZombieSiegeParticipantRepository
{
    public async Task<Result<ZombieSiegeParticipantDto>> GetZombieSiegeParticipantAsync(Guid zombieSiegeParticipantId, CancellationToken cancellationToken)
    {
        var zombieSiegeParticipantById = await context.ZombieSiegeParticipants
            .ProjectTo<ZombieSiegeParticipantDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(zombieSiegeParticipant => zombieSiegeParticipant.Id == zombieSiegeParticipantId,
                cancellationToken);

        return zombieSiegeParticipantById is null
            ? Result.Failure<ZombieSiegeParticipantDto>(ZombieSiegeParticipantErrors.NotFound)
            : Result.Success(zombieSiegeParticipantById);
    }

    public async Task<Result<bool>> InsertZombieSiegeParticipantsAsync(List<CreateZombieSiegeParticipantDto> createZombieSiegeParticipants, CancellationToken cancellationToken)
    {
        var zombieSiegeParticipants = mapper.Map<List<ZombieSiegeParticipant>>(createZombieSiegeParticipants);

        try
        {
            await context.ZombieSiegeParticipants.AddRangeAsync(zombieSiegeParticipants, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(true);
        }
        catch (Exception e)
        {
            logger.LogError(e, "{DateBaseErrorMessage}", e.Message);
            return Result.Failure<bool>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<List<ZombieSiegeParticipantDto>>> GetPlayerZombieSiegeParticipantsAsync(Guid playerId, int last, CancellationToken cancellationToken)
    {
        var playerZombieSieges = await context.ZombieSiegeParticipants
            .Where(zombieSiegeParticipant => zombieSiegeParticipant.PlayerId == playerId)
            .OrderByDescending(zombieSiegeParticipant => zombieSiegeParticipant.ZombieSiege.EventDate)
            .ProjectTo<ZombieSiegeParticipantDto>(mapper.ConfigurationProvider)
            .Take(last)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success(playerZombieSieges);
    }

    public async Task<Result<ZombieSiegeParticipantDto>> UpdateZombieSiegeParticipantAsync(UpdateZombieSiegeParticipantDto updateZombieSiegeParticipantDto,
        CancellationToken cancellationToken)
    {
        var zombieSiegeParticipantToUpdate = await context.ZombieSiegeParticipants
            .FirstOrDefaultAsync(
                zombieSiegeParticipant => zombieSiegeParticipant.Id == updateZombieSiegeParticipantDto.Id,
                cancellationToken);

        if (zombieSiegeParticipantToUpdate is null)
            return Result.Failure<ZombieSiegeParticipantDto>(ZombieSiegeErrors.NotFound);

        mapper.Map(updateZombieSiegeParticipantDto, zombieSiegeParticipantToUpdate);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<ZombieSiegeParticipantDto>(zombieSiegeParticipantToUpdate));
        }
        catch (Exception e)
        {
            logger.LogError(e, "{DateBaseErrorMessage}", e.Message);
            return Result.Failure<ZombieSiegeParticipantDto>(GeneralErrors.DatabaseError);
        }
    }
}