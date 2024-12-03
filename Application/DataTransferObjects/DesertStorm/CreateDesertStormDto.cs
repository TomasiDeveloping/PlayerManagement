using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.DesertStorm;

public class CreateDesertStormDto
{
    [Required]
    public Guid AllianceId { get; set; }

    [Required]
    public bool Won { get; set; }

    [Required]
    public int OpposingParticipants { get; set; }

    [Required]
    public int OpponentServer { get; set; }

    [Required]
    public required string EventDate { get; set; }

    [Required]
    [MaxLength(150)]
    public required string OpponentName { get; set; }

    public bool IsInProgress { get; set; }
}