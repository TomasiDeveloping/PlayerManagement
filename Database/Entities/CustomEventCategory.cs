namespace Database.Entities;

public class CustomEventCategory : BaseEntity
{
    public Guid AllianceId { get; set; }

    public Alliance Alliance { get; set; } = null!;

    public required string Name { get; set; }

    public bool IsPointsEvent { get; set; }

    public bool IsParticipationEvent { get; set; }

    public ICollection<CustomEvent> CustomEvents { get; set; } = [];
}