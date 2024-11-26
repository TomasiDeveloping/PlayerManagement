using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Authentication;

public class ForgotPasswordDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string ResetPasswordUri { get; set; }
}