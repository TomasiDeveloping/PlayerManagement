namespace Application.DataTransferObjects.AllianceAccessToken;

public class CreateAllianceAccessTokenDto
{
    public Guid AllianceId { get; set; }
    public DateTime? ExpiresAt { get; set; }
}