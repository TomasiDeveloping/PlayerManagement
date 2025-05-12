using Application.Classes;
using Application.DataTransferObjects.Stat;

namespace Application.Interfaces;

public interface IStatRepository
{
    Task<Result<AllianceUseToolCount>> GetAllianceUseToolCountAsync(CancellationToken cancellationToken);
}