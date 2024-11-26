namespace Database.Entities;

public class DesertStorm : BaseEntity
{
    public Guid AllianceId { get; set; }

    public Alliance Alliance { get; set; } = null!;

    public DateTime EventDate { get; set; }

    public required string CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public bool Won { get; set; }

    public required string OpponentName { get; set; }

    public int OpponentServer { get; set; }

    public int OpposingParticipants { get; set; }

    public ICollection<DesertStormParticipant> DesertStormParticipants { get; set; } = [];
}