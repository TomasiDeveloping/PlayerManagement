namespace Application.DataTransferObjects.Player;

public class PlayerMvpDto
{
    public required string PlayerName { get; set; }
    public required string Rank { get; set; }
    public long TotalVsDuelPoints { get; set; }
    public int MarshalGuardParticipationCount { get; set; }
    public int DesertStormParticipationCount { get; set; }
    public decimal MvpPoints { get; set; }
}