namespace Application.DataTransferObjects.VsDuelParticipant;

public class VsDuelParticipantDto
{
    public Guid Id { get; set; }

    public Guid PlayerId { get; set; }

    public Guid VsDuelId { get; set; }

    public long WeeklyPoints { get; set; }

    public required string PlayerName { get; set; }
}