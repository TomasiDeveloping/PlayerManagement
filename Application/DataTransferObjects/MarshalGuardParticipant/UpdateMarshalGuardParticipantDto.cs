using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.MarshalGuardParticipant;

public class UpdateMarshalGuardParticipantDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid PlayerId { get; set; }

    [Required]
    public Guid MarshalGuardId { get; set; }

    [Required]
    public bool Participated { get; set; }
}