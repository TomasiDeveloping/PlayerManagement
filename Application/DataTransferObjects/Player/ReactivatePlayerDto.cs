using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Player;

public class ReactivatePlayerDto
{
    [Required]
    public Guid Id { get; set; }
}