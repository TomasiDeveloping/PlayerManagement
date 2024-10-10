using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.VsDuel;

public class UpdateVsDuelDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid PlayerId { get; set; }

    [Required]
    public int WeeklyPoints { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    public int CalendarWeek { get; set; }
}