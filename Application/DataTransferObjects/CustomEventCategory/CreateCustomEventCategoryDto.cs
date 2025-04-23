using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.CustomEventCategory;

public class CreateCustomEventCategoryDto
{
    [Required]
    public Guid AllianceId { get; set; }

    [Required]
    [MaxLength(250)]
    public required string Name { get; set; }

    [Required]
    public bool IsPointsEvent { get; set; }

    [Required]
    public bool IsParticipationEvent { get; set; }
}