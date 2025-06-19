namespace Database.Entities;

public class Squad : BaseEntity
{
    public Guid SquadTypeId { get; set; }

    public SquadType SquadType { get; set; } = null!;

    public decimal Power { get; set; }

    public int Position { get; set; }

    public Guid PlayerId { get; set; }

    public Player Player { get; set; } = null!;

    public DateTime LastUpdateAt { get; set; }
}