namespace Application.DataTransferObjects.DesertStorm;

public class DesertStormDto
{
    public Guid Id { get; set; }

    public bool Registered { get; set; }

    public bool Participated { get; set; }

    public int Year { get; set; }

    public int CalendarWeek { get; set; }
}