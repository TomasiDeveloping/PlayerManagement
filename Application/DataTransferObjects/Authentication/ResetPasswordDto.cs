using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Authentication;

public class ResetPasswordDto
{
    [Required]
    public required string Password { get; set; }

    [Compare("Password")]
    public required string ConfirmPassword { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Token { get; set; }
}