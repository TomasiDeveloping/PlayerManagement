namespace Application.DataTransferObjects.MarshalGuardParticipant;

public class CreateMarshalGuardParticipantDto
{
    public Guid PlayerId { get; set; }

    public Guid MarshalGuardId { get; set; }

    public bool Participated { get; set; }
}