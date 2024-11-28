namespace Application.DataTransferObjects.ZombieSiege;

public class ZombieSiegeDto
{
    public Guid Id { get; set; }

    public int Level { get; set; }

    public int TotalLevel20Players { get; set; }

    public Guid AllianceId { get; set; }

    public DateTime EventDate { get; set; }

    public required string CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public int AllianceSize { get; set; }
}