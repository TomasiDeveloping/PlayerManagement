using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Authentication;

public class SignUpRequestDto
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
    public int AllianceServer { get; set; }

    [Required]
    [MaxLength(200)]
    public required string AllianceName { get; set; }

    [Required]
    [MaxLength(5)]
    public required string AllianceAbbreviation { get; set; }

    [Required]
    public required string EmailConfirmUri { get; set; }
}