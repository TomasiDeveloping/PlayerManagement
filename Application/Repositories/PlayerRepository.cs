using Application.Classes;
using Application.DataTransferObjects.Player;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Repositories;

public class PlayerRepository(ApplicationContext context, IMapper mapper, ILogger<PlayerRepository> logger) : IPlayerRepository
{
    public async Task<Result<PlayerDto>> GetPlayerAsync(Guid playerId, CancellationToken cancellationToken)
    {
        var playerById = await context.Players
            .ProjectTo<PlayerDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(player => player.Id == playerId, cancellationToken);

        return playerById is null
            ? Result.Failure<PlayerDto>(PlayerErrors.NotFound)
            : Result.Success(playerById);
    }

    public async Task<Result<List<PlayerDto>>> GetAlliancePlayersAsync(Guid allianceId, CancellationToken cancellationToken)
    {
        var alliancePlayers = await context.Players
            .Where(player => player.AllianceId == allianceId)
            .ProjectTo<PlayerDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success(alliancePlayers);
    }

    public async Task<Result<PlayerDto>> CreatePlayerAsync(CreatePlayerDto createPlayerDto, CancellationToken cancellationToken)
    {
        var newPlayer = mapper.Map<Player>(createPlayerDto);

        await context.Players.AddAsync(newPlayer, cancellationToken);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<PlayerDto>(newPlayer));
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result.Failure<PlayerDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<PlayerDto>> UpdatePlayerAsync(UpdatePlayerDto updatePlayerDto, CancellationToken cancellationToken)
    {
        var playerToUpdate = await context.Players
            .FirstOrDefaultAsync(player => player.Id == updatePlayerDto.Id, cancellationToken);

        if (playerToUpdate is null) return Result.Failure<PlayerDto>(PlayerErrors.NotFound);

        mapper.Map(updatePlayerDto, playerToUpdate);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<PlayerDto>(playerToUpdate));
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result.Failure<PlayerDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<bool>> DeletePlayerAsync(Guid playerIId, CancellationToken cancellationToken)
    {
        var playerToDelete = await context.Players
            .FirstOrDefaultAsync(player => player.Id == playerIId, cancellationToken);

        if (playerToDelete is null) return Result.Failure<bool>(PlayerErrors.NotFound);

        context.Players.Remove(playerToDelete);

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
}