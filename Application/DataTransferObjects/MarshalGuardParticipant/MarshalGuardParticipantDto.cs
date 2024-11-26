namespace Application.DataTransferObjects.MarshalGuardParticipant;

public class MarshalGuardParticipantDto
{
    public Guid Id { get; set; }

    public Guid PlayerId { get; set; }

    public Guid MarshalGuardId { get; set; }

    public bool Participated { get; set; }

    public required string PlayerName { get; set; }
}