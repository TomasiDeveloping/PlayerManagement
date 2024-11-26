using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.DesertStorm;

public class UpdateDesertStormDto
{
    public Guid Id { get; set; }

    [Required] 
    public bool Won { get; set; }

    [Required] 
    public int OpposingParticipants { get; set; }

    [Required] 
    public int OpponentServer { get; set; }

    public DateTime EventDate { get; set; } = DateTime.Now;

    [Required]
    [MaxLength(150)] 
    public required string OpponentName { get; set; }
}