using Application.DataTransferObjects.DesertStormParticipants;

namespace Application.DataTransferObjects.DesertStorm;

public class DesertStormDetailDto : DesertStormDto
{
    public ICollection<DesertStormParticipantDto> DesertStormParticipants { get; set; } = [];
}