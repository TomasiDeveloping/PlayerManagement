namespace Application.DataTransferObjects.CustomEventCategory;

public class CustomEventCategoryDto
{
    public Guid Id { get; set; }

    public Guid AllianceId { get; set; }

    public required string Name { get; set; }

    public bool IsPointsEvent { get; set; }

    public bool IsParticipationEvent { get; set; }
}