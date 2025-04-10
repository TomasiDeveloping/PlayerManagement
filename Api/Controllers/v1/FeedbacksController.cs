using Application.DataTransferObjects.Feedback;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class FeedbacksController(ILogger<FeedbacksController> logger, IGitHubService gitHubService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<string>> PostFeedback([FromForm] FeedbackDto feedbackDto)
        {
            try
            {
                var issue = await gitHubService.CreateIssueAsync(feedbackDto);
                return Ok(new {url = issue.HtmlUrl});
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(PostFeedback)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

    }
}
