﻿namespace Application.DataTransferObjects.CustomEvent;

public class CustomEventDto
{
    public Guid Id { get; set; }

    public Guid AllianceId { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public bool IsPointsEvent { get; set; }

    public bool IsParticipationEvent { get; set; }

    public DateTime EventDate { get; set; }

    public required string CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

}