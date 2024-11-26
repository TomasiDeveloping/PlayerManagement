namespace Application.DataTransferObjects.MarshalGuard;

public class MarshalGuardDto
{
    public Guid Id { get; set; }

    public Guid AllianceId { get; set; }

    public int Participants { get; set; }

    public int RewardPhase { get; set; }

    public int Level { get; set; }

    public int AllianceSize { get; set; }

    public DateTime EventDate { get; set; }

    public required string CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
}