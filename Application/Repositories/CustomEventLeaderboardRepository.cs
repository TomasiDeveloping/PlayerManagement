using Application.Classes;
using Application.DataTransferObjects.CustomEventLeaderboard;
using Database;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories;

public class CustomEventLeaderboardRepository(ApplicationContext dbContext) : ICustomEventLeaderBoardRepository
{
    public async Task<Result<List<LeaderboardPointEventDto>>> GetPointEventLeaderboardAsync(Guid customEventCategoryId, CancellationToken cancellationToken)
    {
        var category = await dbContext.CustomEventCategories
            .Where(category => category.Id == customEventCategoryId)
            .Include(c => c.CustomEvents)
            .ThenInclude(e => e.CustomEventParticipants)
            .ThenInclude(p => p.Player)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (category == null)
        {
            return Result.Success(new List<LeaderboardPointEventDto>());
        }

        var leaderboard = category.CustomEvents
            .SelectMany(e => e.CustomEventParticipants)
            .GroupBy(p => new { p.Player.Id, p.Player.PlayerName })
            .Select(g => new LeaderboardPointEventDto
            {
                PlayerName = g.Key.PlayerName,
                Points = g.Sum(p => p.AchievedPoints ?? 0)
            })
            .OrderByDescending(l => l.Points)
            .ToList();

        return Result.Success(leaderboard);
    }
    
    public async Task<Result<List<LeaderboardParticipationEventDto>>> GetParticipationEventLeaderboardAsync(Guid customEventCategoryId, CancellationToken cancellationToken)
    {
        var category = await dbContext.CustomEventCategories
            .Where(category => category.Id == customEventCategoryId)
            .Include(c => c.CustomEvents)
            .ThenInclude(e => e.CustomEventParticipants)
            .ThenInclude(p => p.Player)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (category == null)
        {
            return Result.Success(new List<LeaderboardParticipationEventDto>());
        }

        var leaderboard = category.CustomEvents
            .SelectMany(e => e.CustomEventParticipants)
            .GroupBy(p => new { p.Player.Id, p.Player.PlayerName })
            .Select(g => new LeaderboardParticipationEventDto()
            {
                PlayerName = g.Key.PlayerName,
                Participations = g.Count(z => z.Participated!.Value)
            })
            .OrderByDescending(l => l.Participations)
            .ToList();

        return Result.Success(leaderboard);
    }

    public async Task<Result<List<LeaderboardPointAndParticipationEventDto>>> GetPointAndParticipationEventLeaderboardAsync(Guid customEventCategoryId, CancellationToken cancellationToken)
    {
        var category = await dbContext.CustomEventCategories
            .Where(category => category.Id == customEventCategoryId)
            .Include(c => c.CustomEvents)
                .ThenInclude(e => e.CustomEventParticipants)
                    .ThenInclude(p => p.Player)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (category == null)
            return Result.Success(new List<LeaderboardPointAndParticipationEventDto>());

        // Punktetabelle: Platz 1 = 100, Platz 2 = 99, ..., Platz 100 = 1
        var pointTable = Enumerable.Range(1, 100).Select(i => 101 - i).ToArray();

        // Gewichtungen
        const double baseParticipationValue = 10.0;
        const double placeWeight = 2.0;
        const double scoreWeight = 1.0;

        // Dictionaries zur Akkumulation
        var playerPoints = new Dictionary<Guid, long>();
        var playerParticipationCount = new Dictionary<Guid, int>();
        var playerTotalPoints = new Dictionary<Guid, double>();
        var playerNames = new Dictionary<Guid, string>();

        foreach (var ev in category.CustomEvents)
        {
            // Vorab Platzierung berechnen
            var ranked = ev.CustomEventParticipants
                .Where(p => p.Participated == true)
                .OrderByDescending(p => p.AchievedPoints ?? 0)
                .ToList();

            foreach (var participant in ev.CustomEventParticipants)
            {
                if (participant.Player == null) continue;

                var playerId = participant.Player.Id;
                var playerName = participant.Player.PlayerName;

                // Spielername merken – egal ob teilgenommen oder nicht
                playerNames[playerId] = playerName;

                // Nur wenn teilgenommen
                if (participant.Participated == true)
                {
                    var score = participant.AchievedPoints ?? 0;

                    // Index in Platzierungsliste finden
                    var place = ranked.FindIndex(p => p.Player?.Id == playerId);
                    var placePoints = (place >= 0 && place < pointTable.Length) ? pointTable[place] : 1;
                    var normalizedPlace = placePoints / 100.0;
                    var scoreBonus = Math.Log10(score + 1);

                    var total = baseParticipationValue + (normalizedPlace * placeWeight) + (scoreBonus * scoreWeight);

                    playerTotalPoints[playerId] = playerTotalPoints.GetValueOrDefault(playerId) + total;
                    playerParticipationCount[playerId] = playerParticipationCount.GetValueOrDefault(playerId) + 1;
                    playerPoints[playerId] = playerPoints.GetValueOrDefault(playerId) + score;
                }
            }
        }

        // Leaderboard für alle Spieler, die in Events vorkommen
        var leaderboard = playerNames.Select(entry =>
        {
            var playerId = entry.Key;
            var playerName = entry.Value;

            return new LeaderboardPointAndParticipationEventDto
            {
                PlayerName = playerName,
                Points = playerPoints.GetValueOrDefault(playerId),
                Participations = playerParticipationCount.GetValueOrDefault(playerId),
                TotalPoints = Math.Round(playerTotalPoints.GetValueOrDefault(playerId), 2)
            };
        })
        .OrderByDescending(x => x.TotalPoints)
        .ThenByDescending(x => x.Participations)
        .ToList();

        return Result.Success(leaderboard);
    }


}