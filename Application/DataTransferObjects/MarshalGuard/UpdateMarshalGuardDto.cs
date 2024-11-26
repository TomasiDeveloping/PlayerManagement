using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.MarshalGuard;

public class UpdateMarshalGuardDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid AllianceId { get; set; }

    [Required]
    public int Participants { get; set; }

    [Required]
    public int Level { get; set; }

    [Required]
    public int RewardPhase { get; set; }

    [Required]
    public required string EventDate { get; set; }
}