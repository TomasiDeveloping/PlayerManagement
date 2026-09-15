namespace Application.DataTransferObjects.AllianceAccessToken;

public class AllianceAccessTokenDto
{
    public Guid Id { get; set; }
    public Guid AllianceId { get; set; }
    public string Token { get; set; } = null!;
    public string FullShareUrl { get; set; } = null!;
    public DateTime? ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}