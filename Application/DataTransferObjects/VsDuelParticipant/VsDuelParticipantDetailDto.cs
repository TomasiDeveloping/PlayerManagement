namespace Application.DataTransferObjects.VsDuelParticipant;

public class VsDuelParticipantDetailDto
{
    public Guid PlayerId { get; set; }

    public DateTime EventDate { get; set; }

    public long WeeklyPoints { get; set; }
}