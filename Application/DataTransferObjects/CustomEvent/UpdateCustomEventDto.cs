using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.CustomEvent;

public class UpdateCustomEventDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(150)]
    public required string Name { get; set; }

    [Required]
    public bool IsPointsEvent { get; set; }

    [Required]
    public bool IsParticipationEvent { get; set; }

    [Required]
    [MaxLength(500)]
    public required string Description { get; set; }

    [Required]
    public required string EventDateString { get; set; }

}