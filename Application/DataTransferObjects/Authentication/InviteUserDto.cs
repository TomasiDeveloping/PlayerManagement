using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Authentication;

public class InviteUserDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public Guid InvitingUserId { get; set; }

    [Required]
    public Guid AllianceId { get; set; }

    [Required]
    public required string Role { get; set; }

    [Required]
    public required string RegisterUserUri { get; set; }

}