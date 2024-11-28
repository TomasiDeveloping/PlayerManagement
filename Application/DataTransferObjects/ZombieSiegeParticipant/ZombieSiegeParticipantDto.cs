namespace Application.DataTransferObjects.ZombieSiegeParticipant;

public class ZombieSiegeParticipantDto
{
    public Guid Id { get; set; }

    public Guid PlayerId { get; set; }

    public required string PlayerName { get; set; }

    public Guid ZombieSiegeId { get; set; }

    public int SurvivedWaves { get; set; }
}