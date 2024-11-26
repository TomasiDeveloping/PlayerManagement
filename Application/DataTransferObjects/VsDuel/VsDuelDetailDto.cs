using Application.DataTransferObjects.VsDuelParticipant;

namespace Application.DataTransferObjects.VsDuel;

public class VsDuelDetailDto : VsDuelDto
{
    public ICollection<VsDuelParticipantDto> VsDuelParticipants { get; set; } = [];
}