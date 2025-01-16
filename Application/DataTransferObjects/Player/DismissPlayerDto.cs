using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Player;

public class DismissPlayerDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(255)]
    public required string DismissalReason { get; set; }
    
}