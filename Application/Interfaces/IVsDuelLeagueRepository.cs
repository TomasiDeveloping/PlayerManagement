using Application.Classes;
using Application.DataTransferObjects.VsDuelLeague;

namespace Application.Interfaces;

public interface IVsDuelLeagueRepository
{
    Task<Result<List<VsDuelLeagueDto>>> GetVsDuelLeaguesAsync(CancellationToken cancellationToken);
}