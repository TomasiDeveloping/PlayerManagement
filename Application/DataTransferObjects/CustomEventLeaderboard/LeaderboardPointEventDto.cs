namespace Application.DataTransferObjects.CustomEventLeaderboard;

public class LeaderboardPointEventDto
{
    public required string PlayerName { get; set; }

    public long Points { get; set; }
}