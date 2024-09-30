namespace Database.Entities;

public class Note : BaseEntity
{
    public required string PlayerNote { get; set; }

    public Guid PlayerId { get; set; }

    public required Player Player { get; set; }
}