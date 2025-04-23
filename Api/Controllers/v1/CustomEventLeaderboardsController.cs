using Application.DataTransferObjects.CustomEventLeaderboard;
using Application.Repositories;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class CustomEventLeaderboardsController(ICustomEventLeaderBoardRepository customEventLeaderBoardRepository, ILogger<CustomEventLeaderboardsController> logger) : ControllerBase
    {
        [HttpGet("point/{customEventCategoryId:guid}")]
        public async Task<ActionResult<LeaderboardPointEventDto>> GetPointEvent(Guid customEventCategoryId,
            CancellationToken cancellationToken)
        {
            try
            {
                var pointEventLeaderboardResult = await customEventLeaderBoardRepository.GetPointEventLeaderboardAsync(customEventCategoryId, cancellationToken);

                if (pointEventLeaderboardResult.IsFailure) return BadRequest(pointEventLeaderboardResult.Error);

                return pointEventLeaderboardResult.Value.Count > 0
                    ? Ok(pointEventLeaderboardResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetPointEvent)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpGet("participation/{customEventCategoryId:guid}")]
        public async Task<ActionResult<LeaderboardParticipationEventDto>> GetParticipationEvent(Guid customEventCategoryId,
            CancellationToken cancellationToken)
        {
            try
            {
                var participationEventLeaderboardResult = await customEventLeaderBoardRepository.GetParticipationEventLeaderboardAsync(customEventCategoryId, cancellationToken);
                if (participationEventLeaderboardResult.IsFailure) return BadRequest(participationEventLeaderboardResult.Error);
                return participationEventLeaderboardResult.Value.Count > 0
                    ? Ok(participationEventLeaderboardResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetParticipationEvent)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpGet("point-and-participation/{customEventCategoryId:guid}")]
        public async Task<ActionResult<LeaderboardPointAndParticipationEventDto>> GetPointAndParticipationEvent(Guid customEventCategoryId,
            CancellationToken cancellationToken)
        {
            try
            {
                var pointAndParticipationEventLeaderboardResult = await customEventLeaderBoardRepository.GetPointAndParticipationEventLeaderboardAsync(customEventCategoryId, cancellationToken);
                if (pointAndParticipationEventLeaderboardResult.IsFailure) return BadRequest(pointAndParticipationEventLeaderboardResult.Error);
                return pointAndParticipationEventLeaderboardResult.Value.Count > 0
                    ? Ok(pointAndParticipationEventLeaderboardResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetPointAndParticipationEvent)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }
    }
}
