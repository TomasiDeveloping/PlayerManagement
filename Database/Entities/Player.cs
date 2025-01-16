namespace Database.Entities;

public class Player : BaseEntity
{
    public required string PlayerName { get; set; }

    public Rank Rank { get; set; } = null!;

    public Guid RankId { get; set; }

    public Alliance Alliance { get; set; } = null!;

    public Guid AllianceId { get; set; }

    public int Level { get; set; }

    public DateTime CreatedOn { get; set; }

    public required string CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDismissed { get; set; }

    public DateTime? DismissedAt { get; set; }

    public string? DismissalReason { get; set; }

    public ICollection<DesertStormParticipant> DesertStormParticipants { get; set; } = [];

    public ICollection<VsDuelParticipant> VsDuelParticipants { get; set; } = [];

    public ICollection<MarshalGuardParticipant> MarshalGuardParticipants { get; set; } = [];

    public ICollection<Admonition> Admonitions { get; set; } = [];

    public ICollection<Note> Notes { get; set; } = [];

    public ICollection<CustomEventParticipant> CustomEventParticipants { get; set; } = [];

    public ICollection<ZombieSiegeParticipant> ZombieSiegeParticipants { get; set; } = [];
}