namespace Database.Entities;

public class VsDuel : BaseEntity
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

    public long OpponentPower { get; set; }

    public int OpponentSize { get; set; }

    public ICollection<VsDuelParticipant> VsDuelParticipants { get; set; } = [];
}