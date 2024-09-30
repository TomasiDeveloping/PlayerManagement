namespace Database.Entities;

public class DesertStorm : BaseEntity
{
    public bool Registered { get; set; }

    public bool Participated { get; set; }

    public int Year { get; set; }

    public int CalendarWeek { get; set; }


    public Guid PlayerId { get; set; }

    public required Player Player { get; set; }
}