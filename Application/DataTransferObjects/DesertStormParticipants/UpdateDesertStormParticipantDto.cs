using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.DesertStormParticipants;

public class UpdateDesertStormParticipantDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid DesertStormId { get; set; }

    [Required]
    public Guid PlayerId { get; set; }

    [Required]
    public bool Registered { get; set; }

    [Required]
    public bool Participated { get; set; }

    [Required]
    public bool StartPlayer { get; set; }
}