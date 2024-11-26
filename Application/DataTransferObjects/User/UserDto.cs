namespace Application.DataTransferObjects.User;

public class UserDto
{
    public Guid Id { get; set; }

    public Guid AllianceId { get; set; }

    public required string PlayerName { get; set; }

    public required string Email { get; set; }

    public required string Role { get; set; }
}