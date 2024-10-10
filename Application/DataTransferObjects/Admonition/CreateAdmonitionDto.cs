using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Admonition;

public class CreateAdmonitionDto
{
    [Required]
    [MaxLength(250)]
    public required string Reason { get; set; }

    [Required]
    public Guid PlayerId { get; set; }
}