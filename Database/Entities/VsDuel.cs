namespace Database.Entities;

public class VsDuel : BaseEntity
{
    public int WeeklyPoints { get; set; }

    public int Year { get; set; }

    public int CalendarWeek { get; set; }


    public Guid PlayerId { get; set; }

    public required Player Player { get; set; }
}