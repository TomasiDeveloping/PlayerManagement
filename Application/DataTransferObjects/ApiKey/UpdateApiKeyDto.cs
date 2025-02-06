namespace Application.DataTransferObjects.ApiKey;

public class UpdateApiKeyDto
{
    public Guid Id { get; set; }

    public Guid AllianceId { get; set; }
}