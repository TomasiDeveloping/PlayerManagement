namespace Application.DataTransferObjects.DesertStorm;

public class DesertStormDto
{
    public Guid Id { get; set; }

    public bool Won { get; set; }

    public int OpposingParticipants { get; set; }

    public int OpponentServer { get; set; }

    public DateTime EventDate { get; set; }

    public required string OpponentName { get; set; }

    public Guid AllianceId { get; set; }

    public string? ModifiedBy  { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public required string CreatedBy { get; set; }

    public int Participants { get; set; }
}