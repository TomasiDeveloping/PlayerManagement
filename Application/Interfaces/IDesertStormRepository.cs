using Application.Classes;
using Application.DataTransferObjects.DesertStorm;

namespace Application.Interfaces;

public interface IDesertStormRepository
{
    Task<Result<DesertStormDto>> GetDesertStormAsync(Guid desertStormId, CancellationToken cancellationToken);

    Task<Result<List<DesertStormDto>>> GetPlayerDesertStormsAsync(Guid playerId, CancellationToken cancellationToken);

    Task<Result<DesertStormDto>> CreateDesertStormAsync(CreateDesertStormDto createDesertStormDto, CancellationToken cancellationToken);

    Task<Result<DesertStormDto>> UpdateDesertStormAsync(UpdateDesertStormDto updateDesertStormDto, CancellationToken cancellationToken);

    Task<Result<bool>> DeleteDesertStormAsync(Guid desertStormId, CancellationToken cancellationToken);
}