using Application.Classes;
using Application.DataTransferObjects.CustomEventLeaderboard;


namespace Application.Repositories;

public interface ICustomEventLeaderBoardRepository
{
    Task<Result<List<LeaderboardPointEventDto>>> GetPointEventLeaderboardAsync(Guid customEventCategoryId, CancellationToken cancellationToken);

    Task<Result<List<LeaderboardParticipationEventDto>>> GetParticipationEventLeaderboardAsync(Guid customEventCategoryId, CancellationToken cancellationToken);

    Task<Result<List<LeaderboardPointAndParticipationEventDto>>> GetPointAndParticipationEventLeaderboardAsync(Guid customEventCategoryId, CancellationToken cancellationToken);
}