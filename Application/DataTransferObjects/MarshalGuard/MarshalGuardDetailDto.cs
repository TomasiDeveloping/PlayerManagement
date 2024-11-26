using Application.DataTransferObjects.MarshalGuardParticipant;

namespace Application.DataTransferObjects.MarshalGuard;

public class MarshalGuardDetailDto : MarshalGuardDto
{
    public ICollection<MarshalGuardParticipantDto> MarshalGuardParticipants { get; set; } = [];
}