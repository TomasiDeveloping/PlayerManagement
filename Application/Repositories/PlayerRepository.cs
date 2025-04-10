using Application.Classes;
using Application.DataTransferObjects;
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

    public async Task<Result<PagedResponseDto<PlayerDto>>> GetAllianceDismissPlayersAsync(Guid allianceId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = context.Players
            .IgnoreQueryFilters()
            .Where(player => player.AllianceId == allianceId && player.IsDismissed)
            .OrderByDescending(player => player.DismissedAt)
            .AsNoTracking();

        var totalRecord = await query.CountAsync(cancellationToken);

        var pagedDismissPlayers = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<PlayerDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);


        return Result.Success(new PagedResponseDto<PlayerDto>
        {
            Data = pagedDismissPlayers,
            TotalRecords = totalRecord,
            PageSize = pageSize,
            PageNumber = pageNumber
        });
    }

    public async Task<Result<List<PlayerMvpDto>>> GetAllianceMvp(Guid allianceId, string? playerType, CancellationToken cancellationToken)
    {
        var currentDate = DateTime.Now;
        var threeWeeksAgo = currentDate.AddDays(-21);

        var query = context.Players.Where(p => p.AllianceId == allianceId);

        query = playerType switch
        {
            "players" => query.Where(p => p.Rank.Name != "R4" && p.Rank.Name != "R5"),
            "leadership" => query.Where(p => p.Rank.Name == "R4" || p.Rank.Name == "R5"),
            _ => query
        };

        var playerMvps = await query
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

                IsOldestVsDuelParticipated = context.VsDuelParticipants
                    .Where(vp => vp.PlayerId == p.Id && vp.VsDuel.EventDate <= currentDate && !vp.VsDuel.IsInProgress)
                    .OrderByDescending(vp => vp.VsDuel.EventDate)
                    .Skip(2)
                    .Take(1)
                    .Any(),

                MarshalGuardParticipationCount = context.MarshalGuardParticipants
                    .Count(mpg => mpg.PlayerId == p.Id && mpg.Participated && mpg.MarshalGuard.EventDate > threeWeeksAgo),

                DessertStormParticipationCount = context.DesertStormParticipants
                    .Count(dsp => dsp.PlayerId == p.Id && dsp.Participated && dsp.DesertStorm.EventDate > threeWeeksAgo)
            })
            .Select(p => new PlayerMvpDto()
            {
                Name = p.PlayerName,
                AllianceRank = p.Rank,
                DuelPointsLast3Weeks = p.VsDuels,
                MarshalParticipationCount = p.MarshalGuardParticipationCount,
                DesertStormParticipationCount = p.DessertStormParticipationCount,
                HasParticipatedInOldestDuel = p.IsOldestVsDuelParticipated,
                MvpScore = Math.Round(
                    (decimal)((p.VsDuels / 1000000.0 * 0.8) +
                              ((p.MarshalGuardParticipationCount * 20 + p.DessertStormParticipationCount * 40) * 0.2)), 2)
            })
            .OrderByDescending(p => p.MvpScore)
            .ThenByDescending(p => p.DuelPointsLast3Weeks)
            .ThenByDescending(p => p.MarshalParticipationCount)
            .ThenBy(p => p.Name)
            .ToListAsync(cancellationToken);

        return playerMvps;
    }

    public async Task<Result<DismissPlayerInformationDto>> GetDismissPlayerInformationAsync(Guid playerId, CancellationToken cancellationToken)
    {
        var dismissPlayerInformation = await context.Players
            .IgnoreQueryFilters()
            .ProjectTo<DismissPlayerInformationDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(player => player.Id == playerId, cancellationToken);

        return dismissPlayerInformation is null
            ? Result.Failure<DismissPlayerInformationDto>(PlayerErrors.NotFound)
            : Result.Success(dismissPlayerInformation);
    }

    public async Task<Result<PlayerDto>> CreatePlayerAsync(CreatePlayerDto createPlayerDto, string createdBy, CancellationToken cancellationToken)
    {
        var newPlayer = new Player()
        {
            CreatedBy = createdBy,
            PlayerName = createPlayerDto.PlayerName,
            AllianceId = createPlayerDto.AllianceId,
            RankId = createPlayerDto.RankId,
            Level = createPlayerDto.Level,
            CreatedOn = DateTime.Now,
            ModifiedOn = null,
            ModifiedBy = null,
            DismissalReason = null,
            DismissedAt = null,
            IsDismissed = false,
            Id = Guid.CreateVersion7()
        };

        await context.Players.AddAsync(newPlayer, cancellationToken);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<PlayerDto>(newPlayer));
        }
        catch (Exception e)
        {
            logger.LogError(e, "{DateBaseErrorMessage}", e.Message);
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
            logger.LogError(e, "{DateBaseErrorMessage}", e.Message);
            return Result.Failure<PlayerDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<PlayerDto>> DismissPlayerAsync(DismissPlayerDto dismissPlayerDto, string modifiedBy, CancellationToken cancellationToken)
    {
        var playerToDismiss = await context.Players
            .FirstOrDefaultAsync(player => player.Id == dismissPlayerDto.Id, cancellationToken);

        if (playerToDismiss is null) return Result.Failure<PlayerDto>(PlayerErrors.NotFound);

        playerToDismiss.IsDismissed = true;
        playerToDismiss.DismissedAt = DateTime.Now;
        playerToDismiss.DismissalReason = dismissPlayerDto.DismissalReason;
        playerToDismiss.ModifiedOn = DateTime.Now;
        playerToDismiss.ModifiedBy = modifiedBy;

        playerToDismiss.Admonitions.Add(new Admonition()
        {
            CreatedBy = modifiedBy,
            Reason = $"Player was dismiss from the alliance by {modifiedBy}. Reason: {dismissPlayerDto.DismissalReason}",
            CreatedOn = DateTime.Now,
            PlayerId = playerToDismiss.Id,
            Id = Guid.CreateVersion7()
        });

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<PlayerDto>(playerToDismiss));
        }
        catch (Exception e)
        {
            logger.LogError(e, "{DateBaseErrorMessage}", e.Message);
            return Result.Failure<PlayerDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<PlayerDto>> ReactivatePlayerAsync(ReactivatePlayerDto reactivatePlayerDto, string modifiedBy, CancellationToken cancellationToken)
    {
        var playerToReactivate = await context.Players
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(player => player.Id == reactivatePlayerDto.Id, cancellationToken);

        if (playerToReactivate is null) return Result.Failure<PlayerDto>(PlayerErrors.NotFound);

        playerToReactivate.IsDismissed = false;
        playerToReactivate.DismissedAt = null;
        playerToReactivate.DismissalReason = null;
        playerToReactivate.ModifiedOn = DateTime.Now;
        playerToReactivate.ModifiedBy = modifiedBy;

        playerToReactivate.Notes.Add(new Note()
        {
            CreatedBy = modifiedBy,
            PlayerNote = $"Player was accepted back into the alliance by {modifiedBy}",
            CreatedOn = DateTime.Now,
            Id = Guid.CreateVersion7()
        });

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<PlayerDto>(playerToReactivate));
        }
        catch (Exception e)
        {
            logger.LogError(e, "{DateBaseErrorMessage}", e.Message);
            return Result.Failure<PlayerDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<bool>> DeletePlayerAsync(Guid playerIId, CancellationToken cancellationToken)
    {
        var playerToDelete = await context.Players
            .IgnoreQueryFilters()
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
            logger.LogError(e, "{DateBaseErrorMessage}", e.Message);
            return Result.Failure<bool>(GeneralErrors.DatabaseError);
        }
    }
}