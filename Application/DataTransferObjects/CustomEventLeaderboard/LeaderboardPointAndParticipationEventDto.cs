namespace Application.DataTransferObjects.CustomEventLeaderboard;

public class LeaderboardPointAndParticipationEventDto
{
    public required string PlayerName { get; set; }

    public long Points { get; set; }

    public int Participations { get; set; }

    public double TotalPoints { get; set; }
}