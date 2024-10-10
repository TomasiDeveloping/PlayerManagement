namespace Application.DataTransferObjects.DesertStorm;

public class UpdateDesertStormDto
{
    public Guid Id { get; set; }

    public Guid PlayerId { get; set; }

    public bool Registered { get; set; }

    public bool Participated { get; set; }

    public int Year { get; set; }

    public int CalendarWeek { get; set; }
}