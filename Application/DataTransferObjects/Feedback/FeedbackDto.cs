using Microsoft.AspNetCore.Http;

namespace Application.DataTransferObjects.Feedback;

public class FeedbackDto
{
    public required string Type { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public string? ExpectedBehavior { get; set; }
    public string? ActualBehavior { get; set; }
    public string? Reproduction { get; set; }
    public string? Severity { get; set; }
    public string? Os { get; set; }
    public required string AppVersion { get; set; }
    public string? Email { get; set; }
    public IFormFile? Screenshot { get; set; }
}