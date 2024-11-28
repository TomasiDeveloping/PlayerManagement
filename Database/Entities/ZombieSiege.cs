namespace Database.Entities;

public class ZombieSiege : BaseEntity
{
    public int Level { get; set; }

    public Guid AllianceId { get; set; }

    public Alliance Alliance { get; set; } = null!;

    public DateTime EventDate { get; set; }

    public required string CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public int AllianceSize { get; set; }

    public ICollection<ZombieSiegeParticipant> ZombieSiegeParticipants { get; set; } = [];
}