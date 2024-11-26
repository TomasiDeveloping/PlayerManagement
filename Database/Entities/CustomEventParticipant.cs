namespace Database.Entities;

public class CustomEventParticipant : BaseEntity
{
    public Player Player { get; set; } = null!;

    public Guid PlayerId { get; set; }

    public CustomEvent CustomEvent { get; set; } = null!;

    public Guid CustomEventId { get; set; }

    public bool? Participated { get; set; }

    public long? AchievedPoints { get; set; }
}