using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Admonition;

public class UpdateAdmonitionDto
{
    [Required]
    public Guid Id { get; set; }


    [Required]
    [MaxLength(250)]
    public required string Reason { get; set; }
}