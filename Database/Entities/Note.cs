namespace Database.Entities;

public class Note : BaseEntity
{
    public required string PlayerNote { get; set; }

    public DateTime CreatedOn{ get; set; }

    public required string CreatedBy { get; set; }

    public DateTime? ModifiedOn  { get; set; }

    public string? ModifiedBy { get; set; }

    public Guid PlayerId { get; set; }

    public Player Player { get; set; } = null!;
}