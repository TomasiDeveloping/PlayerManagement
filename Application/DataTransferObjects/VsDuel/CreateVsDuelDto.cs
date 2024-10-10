using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.VsDuel;

public class CreateVsDuelDto
{
    [Required]
    public Guid PlayerId { get; set; }
    
    [Required]
    public int WeeklyPoints { get; set; }

}