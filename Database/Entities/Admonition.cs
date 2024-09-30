namespace Database.Entities;

public class Admonition : BaseEntity
{
    public required string Reason { get; set; }

    public Guid PlayerId { get; set; }

    public required Player Player { get; set; }
}