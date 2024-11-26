using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Player;

public class UpdatePlayerDto
{
    public Guid Id{ get; set; }
    [Required]
    [MaxLength(250)]
    public required string PlayerName { get; set; }

    [Required]
    public Guid RankId { get; set; }

    [Required]
    public int Level { get; set; }
}