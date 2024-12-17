using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.CustomEventParticipant;

public class UpdateCustomEventParticipantDto
{
    [Required] public Guid Id { get; set; }

    [Required]
    public Guid CustomEventId { get; set; }

    [Required]
    public Guid PlayerId { get; set; }

    public bool? Participated { get; set; }

    public long? AchievedPoints { get; set; }
}