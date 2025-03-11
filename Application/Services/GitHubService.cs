using Application.DataTransferObjects.Feedback;
using Application.Interfaces;
using Microsoft.Extensions.Options;
using Octokit;
using Utilities.Classes;
using ProductHeaderValue = Octokit.ProductHeaderValue;

namespace Application.Services;

public class GitHubService(IOptions<GitHubSetting> gitHubSettingOption) : IGitHubService
{
    private readonly GitHubSetting _gitHubSetting = gitHubSettingOption.Value;

    public async Task<Issue> CreateIssueAsync(FeedbackDto feedbackDto)
    {
        var gitHubClient = new GitHubClient(new ProductHeaderValue("PlayerManagerApi"))
        {
            Credentials = new Credentials(_gitHubSetting.Token)
        };

        var label = feedbackDto.Type == "bug" ? "bug" : "enhancement";
        var issueBody = $"**Description:**\n{feedbackDto.Description}\n\n";
     
        if (label == "bug")
        {
            issueBody += $"**Expected Behavior:**\n{feedbackDto.ExpectedBehavior}\n\n" +
                         $"**Actual Behavior:**\n{feedbackDto.ActualBehavior}\n\n" +
                         $"**Steps to Reproduce:**\n{feedbackDto.Reproduction}\n\n" +
                         $"**Severity:** {feedbackDto.Severity}\n\n" +
                         $"**Operating System:** {feedbackDto.Os}\n\n" +
                         $"**App Version:** {feedbackDto.AppVersion}\n\n";
        }

        if (feedbackDto.Email is not null)
        {
            issueBody += $"**Contact:** {feedbackDto.Email}\n\n";
        }

        if (feedbackDto.Screenshot is { Length: > 0 })
        {
            using var stream = new MemoryStream();
            await feedbackDto.Screenshot.CopyToAsync(stream);
            var fileBytes = stream.ToArray();

            var filePath = $"screenshots/{Guid.NewGuid()}.png";
            var createFileRequest = new CreateFileRequest(
                "Upload Screenshot",
                Convert.ToBase64String(fileBytes),
                false
            );

            var fileResponse = await gitHubClient.Repository.Content.CreateFile(
                _gitHubSetting.Owner,
                _gitHubSetting.Name,
                filePath,
                createFileRequest
            );

            issueBody += $"**Screenshot:**\n\n ![Screenshot]({fileResponse.Content.DownloadUrl})";
        }

        var createIssue = new NewIssue($"[{feedbackDto.Type.ToUpper()}] {feedbackDto.Title}")
        {
            Body = issueBody
        };
        createIssue.Labels.Add(label);

        return await gitHubClient.Issue.Create(_gitHubSetting.Owner, _gitHubSetting.Name, createIssue);
    }
}