using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Player;

public class CreatePlayerDto
{
    [Required]
    [MaxLength(250)]
    public required string PlayerName { get; set; }

    [Required]
    public Guid RankId { get; set; }

    [Required]
    public Guid AllianceId { get; set; }

    [Required]
    [MaxLength(3)]
    public required string Level { get; set; }
}