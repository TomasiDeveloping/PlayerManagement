using Application.Classes;
using Application.DataTransferObjects.Rank;

namespace Application.Interfaces;

public interface IRankRepository
{
    Task<Result<List<RankDto>>> GetRanksAsync(CancellationToken cancellationToken);
}