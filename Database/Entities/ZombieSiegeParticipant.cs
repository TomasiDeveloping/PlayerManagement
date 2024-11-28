namespace Database.Entities;

public class ZombieSiegeParticipant : BaseEntity
{
    public Player Player { get; set; } = null!;

    public Guid PlayerId { get; set; }

    public Guid ZombieSiegeId { get; set; }

    public ZombieSiege ZombieSiege { get; set; } = null!;

    public int SurvivedWaves { get; set; }
}