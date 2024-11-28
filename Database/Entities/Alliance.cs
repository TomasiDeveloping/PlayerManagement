namespace Database.Entities;

public class Alliance : BaseEntity
{
    public int Server { get; set; }

    public required string Name { get; set; }

    public required string Abbreviation { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public ICollection<Player> Players { get; set; } = [];

    public ICollection<User> Users { get; set; } = [];

    public ICollection<DesertStorm> DesertStorms { get; set; } = [];

    public ICollection<CustomEvent> CustomEvents { get; set; } = [];

    public ICollection<MarshalGuard> MarshalGuards { get; set; } = [];

    public ICollection<VsDuel> VsDuels { get; set; } = [];

    public ICollection<ZombieSiege> ZombieSieges { get; set; } = [];
}