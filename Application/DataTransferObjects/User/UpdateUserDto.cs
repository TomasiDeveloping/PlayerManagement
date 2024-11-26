using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.User;

public class UpdateUserDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public required string PlayerName { get; set; }

    [Required]
    public required string Role { get; set; }
}