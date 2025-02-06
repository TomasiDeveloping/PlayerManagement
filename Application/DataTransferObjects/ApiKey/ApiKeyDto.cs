namespace Application.DataTransferObjects.ApiKey;

public class ApiKeyDto
{
    public Guid Id { get; set; }
    public Guid AllianceId { get; set; }

    public required string Key { get; set; }

    public DateTime CreatedOn { get; set; }

    public required string CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
}