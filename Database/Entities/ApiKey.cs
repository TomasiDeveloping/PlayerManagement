namespace Database.Entities;

public class ApiKey : BaseEntity
{
    public Alliance Alliance { get; set; } = null!;

    public Guid AllianceId { get; set; }

    public required string EncryptedKey { get; set; }

    public DateTime CreatedOn { get; set; }

    public required string CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
}