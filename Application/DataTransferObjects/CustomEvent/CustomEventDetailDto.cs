using Application.DataTransferObjects.CustomEventParticipant;

namespace Application.DataTransferObjects.CustomEvent;

public class CustomEventDetailDto : CustomEventDto
{
    public ICollection<CustomEventParticipantDto> CustomEventParticipants { get; set; } = [];
}