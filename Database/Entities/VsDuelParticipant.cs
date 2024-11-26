namespace Database.Entities;

public class VsDuelParticipant : BaseEntity
{
    public Player Player { get; set; } = null!;

    public Guid PlayerId { get; set; }

    public VsDuel VsDuel { get; set; } = null!;

    public Guid VsDuelId { get; set; }

    public long WeeklyPoints { get; set; }
}