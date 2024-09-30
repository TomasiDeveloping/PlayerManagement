namespace Database.Entities;

public class Rank : BaseEntity
{
    public required string Name { get; set; }

    public Guid PlayerId { get; set; }

    public required Player Player { get; set; }
}