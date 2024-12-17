using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.CustomEventParticipant;

public class CreateCustomEventParticipantDto
{
    [Required]
    public Guid CustomEventId { get; set; }

    [Required]
    public Guid PlayerId { get; set; }

    public bool? Participated { get; set; }

    public long? AchievedPoints { get; set; }
}