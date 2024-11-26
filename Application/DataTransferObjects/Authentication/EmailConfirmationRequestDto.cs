using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Authentication;

public class EmailConfirmationRequestDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string ClientUri { get; set; }
}