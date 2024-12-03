namespace Database.Entities;

public class CustomEvent : BaseEntity
{
    public Guid AllianceId { get; set; }

    public Alliance Alliance { get; set; } = null!;

    public required string Name { get; set; }

    public required string Description { get; set; }

    public bool IsPointsEvent { get; set; }

    public bool IsParticipationEvent { get; set; }

    public bool IsInProgress { get; set; }

    public DateTime EventDate { get; set; }

    public required string CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public ICollection<CustomEventParticipant> CustomEventParticipants { get; set; } = [];
}