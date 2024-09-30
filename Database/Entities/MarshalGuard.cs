namespace Database.Entities;

public class MarshalGuard : BaseEntity
{
    public bool Participated { get; set; }

    public int Year { get; set; }

    public int Month { get; set; }

    public int Day { get; set; }

    public Guid PlayerId { get; set; }

    public required Player Player { get; set; }
}