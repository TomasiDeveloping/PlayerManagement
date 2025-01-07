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

    public async Task<Result<List<PlayerMvpDto>>> GetAlliancePlayersMvp(Guid allianceId, CancellationToken cancellationToken)
    {
        var currentDate = DateTime.Now;
        var threeWeeksAgo = currentDate.AddDays(-21);

        var playerMvps = await context.Players
            .Where(p => p.AllianceId == allianceId && p.Rank.Name != "R4" && p.Rank.Name != "R5")
            .Select(p => new
            {
                p.Id,
                p.PlayerName,
                Rank = p.Rank.Name,

                VsDuels = context.VsDuelParticipants
                    .Where(vp => vp.PlayerId == p.Id && vp.VsDuel.EventDate <= currentDate && !vp.VsDuel.IsInProgress)
                    .OrderByDescending(vp => vp.VsDuel.EventDate)
                    .Take(3)
                    .Sum(vp => vp.WeeklyPoints),

                MarshalGuardParticipationCount = context.MarshalGuardParticipants
                    .Count(mpg => mpg.PlayerId == p.Id && mpg.Participated && mpg.MarshalGuard.EventDate > threeWeeksAgo),

                DessertStormParticipationCount = context.DesertStormParticipants
                    .Count(dsp => dsp.PlayerId == p.Id && dsp.Participated && dsp.DesertStorm.EventDate > threeWeeksAgo)
            })
            .Select(p => new PlayerMvpDto()
            {
                PlayerName = p.PlayerName,
                Rank = p.Rank,
                TotalVsDuelPoints = p.VsDuels,
                MarshalGuardParticipationCount = p.MarshalGuardParticipationCount,
                DesertStormParticipationCount = p.DessertStormParticipationCount,
                MvpPoints = Math.Round(
                    (decimal)((p.VsDuels / 1000000.0 * 0.7) +
                              (p.MarshalGuardParticipationCount * 15) +
                              (p.DessertStormParticipationCount * 10))
                )
            })
            .OrderByDescending(p => p.MvpPoints)
            .ThenByDescending(p => p.TotalVsDuelPoints)
            .ThenByDescending(p => p.MarshalGuardParticipationCount)
            .ThenBy(p => p.PlayerName)
            .ToListAsync(cancellationToken);

        return playerMvps;
    }

    public async Task<Result<PlayerDto>> CreatePlayerAsync(CreatePlayerDto createPlayerDto, string createdBy, CancellationToken cancellationToken)
    {
        var newPlayer = mapper.Map<Player>(createPlayerDto);
        newPlayer.CreatedBy = createdBy;

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

    public async Task<Result<PlayerDto>> UpdatePlayerAsync(UpdatePlayerDto updatePlayerDto, string modifiedBy, CancellationToken cancellationToken)
    {
        var playerToUpdate = await context.Players
            .FirstOrDefaultAsync(player => player.Id == updatePlayerDto.Id, cancellationToken);

        if (playerToUpdate is null) return Result.Failure<PlayerDto>(PlayerErrors.NotFound);

        mapper.Map(updatePlayerDto, playerToUpdate);
        playerToUpdate.ModifiedBy = modifiedBy;

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

        var customEvents = await context.CustomEventParticipants
            .Where(customEvent => customEvent.PlayerId == playerToDelete.Id)
            .ToListAsync(cancellationToken);

        if (customEvents.Count > 0) context.CustomEventParticipants.RemoveRange(customEvents);

        var desertStorms = await context.DesertStormParticipants
            .Where(desertStorm => desertStorm.PlayerId == playerToDelete.Id)
            .ToListAsync(cancellationToken);

        if (desertStorms.Count > 0) context.DesertStormParticipants.RemoveRange(desertStorms);

        var vsDuels = await context.VsDuelParticipants
            .Where(vsDuel => vsDuel.PlayerId == playerToDelete.Id)
            .ToListAsync(cancellationToken);

        if (vsDuels.Count > 0) context.VsDuelParticipants.RemoveRange(vsDuels);

        var marshalGuards = await context.MarshalGuardParticipants
            .Where(marshalGuard => marshalGuard.PlayerId == playerToDelete.Id)
            .ToListAsync(cancellationToken);

        if (vsDuels.Count > 0) context.MarshalGuardParticipants.RemoveRange(marshalGuards);

        var zombieSieges = await context.ZombieSiegeParticipants
            .Where(zombieSiege => zombieSiege.PlayerId == playerToDelete.Id)
            .ToListAsync(cancellationToken);

        if (zombieSieges.Count > 0) context.ZombieSiegeParticipants.RemoveRange(zombieSieges);

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