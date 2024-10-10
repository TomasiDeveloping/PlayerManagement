using Microsoft.AspNetCore.Identity;

namespace Database.Entities;

public class User : IdentityUser<Guid>
{
    public required Alliance Alliance { get; set; }

    public Guid AllianceId { get; set; }

    public required string PlayerName { get; set; }
}