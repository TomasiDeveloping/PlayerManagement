namespace Application.DataTransferObjects.CustomEventParticipant;

public class CustomEventParticipantDto
{
    public Guid Id { get; set; }

    public Guid PlayerId { get; set; }

    public Guid CustomEventId { get; set; }

    public bool? Participated { get; set; }

    public long? AchievedPoints { get; set; }

    public required string PlayerName { get; set; }
}