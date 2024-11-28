using Application.DataTransferObjects.ZombieSiegeParticipant;

namespace Application.DataTransferObjects.ZombieSiege;

public class ZombieSiegeDetailDto : ZombieSiegeDto
{
    public ICollection<ZombieSiegeParticipantDto> ZombieSiegeParticipants { get; set; } = [];
}