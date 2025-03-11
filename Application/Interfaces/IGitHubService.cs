using Application.DataTransferObjects.Feedback;
using Octokit;

namespace Application.Interfaces;

public interface IGitHubService
{
    public Task<Issue> CreateIssueAsync(FeedbackDto feedbackDto);
}