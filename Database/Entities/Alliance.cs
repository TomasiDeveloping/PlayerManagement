namespace Database.Entities;

public class Alliance : BaseEntity
{
    public int Server { get; set; }

    public required string Name { get; set; }

    public required string Abbreviation { get; set; }

    public User User { get; set; }

    public Guid UserId { get; set; }

    public ICollection<Player> Players { get; set; }
}