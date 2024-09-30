namespace Database.Entities;

public class Player : BaseEntity
{
    public required string PlayerName { get; set; }

    public required Rank Rank { get; set; }

    public Alliance Alliance { get; set; }

    public required string Level { get; set; }

    public ICollection<DesertStorm> DesertStorms { get; set; } = [];

    public ICollection<VsDuel> VsDuels { get; set; } = [];

    public ICollection<MarshalGuard> MarshalGuards { get; set; } = [];

    public ICollection<Admonition> Admonitions { get; set; } = [];


    public ICollection<Note> Notes { get; set; } = [];
}