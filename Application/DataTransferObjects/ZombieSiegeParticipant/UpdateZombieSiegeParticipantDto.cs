﻿using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.ZombieSiegeParticipant;

public class UpdateZombieSiegeParticipantDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid PlayerId { get; set; }

    [Required]
    public Guid ZombieSiegeId { get; set; }

    [Required]
    public int SurvivedWaves { get; set; }
}