namespace Application.DataTransferObjects.PlayerCombatRecord;

public class PlayerCombatDto
{
    public required string AllianceName { get; set; }

    public ICollection<CombatRecordPlayer> Players { get; set; } = new List<CombatRecordPlayer>();
}