using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.User;

public class ChangePasswordDto
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public required string CurrentPassword { get; set; }

    [Required]
    public required string NewPassword { get; set; }

    [Required]
    public required string ConfirmPassword { get; set; }
}