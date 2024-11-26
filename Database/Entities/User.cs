using Microsoft.AspNetCore.Identity;

namespace Database.Entities;

public class User : IdentityUser<Guid>
{
    public Alliance Alliance { get; set; } = null!;

    public Guid AllianceId { get; set; }

    public required string PlayerName { get; set; }
}