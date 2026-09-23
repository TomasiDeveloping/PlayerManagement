namespace Application.DataTransferObjects.PlayerCombatRecord;

public class UpdatePlayerCombatDto
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }

    public required string Squad1 { get; set; }
    public decimal Squad1Power { get; set; }

    public required string Squad2 { get; set; }
    public decimal Squad2Power { get; set; }

    public required string Squad3 { get; set; }
    public decimal Squad3Power { get; set; }

    public decimal TotalHeroPower { get; set; }
    public decimal Kills { get; set; }

    public DateTime RecordedAtUtc { get; set; }
}