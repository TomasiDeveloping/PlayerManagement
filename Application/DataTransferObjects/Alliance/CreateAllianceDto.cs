﻿using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Alliance;

public class CreateAllianceDto
{
    [Required]
    public int Server { get; set; }

    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(5)]
    public required string Abbreviation { get; set; }
}