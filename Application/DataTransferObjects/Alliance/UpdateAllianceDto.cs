using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Alliance;

public class UpdateAllianceDto
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(5)]
    public required string Abbreviation { get; set; }
}