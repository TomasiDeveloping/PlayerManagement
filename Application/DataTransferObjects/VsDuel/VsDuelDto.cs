using Database.Entities;

namespace Application.DataTransferObjects.VsDuel;

public class VsDuelDto
{
    public Guid Id { get; set; }

    public Guid AllianceId { get; set; }

    public DateTime EventDate { get; set; }

    public required string CreatedBy { get; set; }

    public bool Won { get; set; }

    public required string OpponentName { get; set; }

    public int OpponentServer { get; set; }

    public long OpponentPower { get; set; }

    public int OpponentSize { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

}