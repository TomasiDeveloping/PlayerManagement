namespace Database.Entities;

public class MarshalGuardParticipant : BaseEntity
{
    public Player Player { get; set; } = null!;

    public Guid PlayerId { get; set; }

    public MarshalGuard MarshalGuard { get; set; } = null!;

    public Guid MarshalGuardId { get; set; }

    public bool Participated { get; set; }

}