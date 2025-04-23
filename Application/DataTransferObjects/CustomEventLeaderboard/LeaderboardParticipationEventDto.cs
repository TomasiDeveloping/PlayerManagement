namespace Application.DataTransferObjects.CustomEventLeaderboard;

public class LeaderboardParticipationEventDto
{
    public required string PlayerName { get; set; }

    public int Participations { get; set; }
}