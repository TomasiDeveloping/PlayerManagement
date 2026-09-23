using Database;

namespace Application.DataTransferObjects.PlayerCombatRecord;

public class PlayerCombatRecordOverviewDto
{
    public Guid PlayerId { get; set; }

    public required string PlayerName { get; set; }

    public int? Squad1 { get; set; }
    public decimal Squad1Power { get; set; }

    public int? Squad2 { get; set; }
    public decimal Squad2Power { get; set; }

    public int? Squad3 { get; set; }
    public decimal Squad3Power { get; set; }

    public decimal TotalHeroPower { get; set; }
    public decimal Kills { get; set; }

    public DateTime? RecordedAtUtc { get; set; }
}