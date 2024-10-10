namespace Database.Entities;

public class Rank : BaseEntity
{
    public required string Name { get; set; }

    public ICollection<Player> Players { get; set; } = [];
}