using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Authentication;

public class RegisterUserDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [MaxLength(200)]
    public required string PlayerName { get; set; }

    [Required]
    public required string Password { get; set; }

    [Required]
    public Guid AllianceId { get; set; }

    [Required]
    public Guid RoleId { get; set; }

    [Required]
    public required string EmailConfirmUri { get; set; }
}