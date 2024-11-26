using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Authentication;

public class ConfirmEmailRequestDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Token { get; set; }
}