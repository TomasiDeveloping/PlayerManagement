using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.VsDuel;

public class UpdateVsDuelDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid AllianceId { get; set; }

    [Required]
    public Guid VsDuelLeagueId { get; set; }

    [Required]
    public required string EventDate { get; set; }

    [Required]
    public bool Won { get; set; }

    [Required]
    [MaxLength(150)]
    public required string OpponentName { get; set; }

    [Required]
    public int OpponentServer { get; set; }

    [Required]
    public long OpponentPower { get; set; }

    [Required]
    public int OpponentSize { get; set; }

    [Required]
    public bool IsInProgress { get; set; }
}