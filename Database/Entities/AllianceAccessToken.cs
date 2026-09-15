namespace Database.Entities;

public class AllianceAccessToken : BaseEntity
{
    public Guid AllianceId { get; set; }
    public Alliance Alliance { get; set; } = null!;

    public required string Token { get; set; }

    public DateTime? ExpiresAtUtc { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}