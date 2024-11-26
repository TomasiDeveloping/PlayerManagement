using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.MarshalGuard;

public class CreateMarshalGuardDto
{
    [Required]
    public Guid AllianceId { get; set; }

    [Required]
    public int RewardPhase { get; set; }

    [Required]
    public int Level { get; set; }

    [Required]
    public int AllianceSize { get; set; }

    [Required]
    public required string EventDate { get; set; }

}