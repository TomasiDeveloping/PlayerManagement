using Application.Classes;
using Application.DataTransferObjects.DesertStorm;

namespace Application.Interfaces;

public interface IDesertStormRepository
{
    Task<Result<DesertStormDto>> GetDesertStormAsync(Guid desertStormId, CancellationToken cancellationToken);

    Task<Result<List<DesertStormDto>>> GetAllianceDesertStormsAsync(Guid allianceId, int take, CancellationToken cancellationToken);

    Task<Result<DesertStormDetailDto>> GetDesertStormDetailAsync(Guid desertStormId, CancellationToken cancellationToken);

    Task<Result<DesertStormDto>> CreateDesertStormAsync(CreateDesertStormDto createDesertStormDto, string createdBy,  CancellationToken cancellationToken);

    Task<Result<DesertStormDto>> UpdateDesertStormAsync(UpdateDesertStormDto updateDesertStormDto, string modifiedBy, CancellationToken cancellationToken);

    Task<Result<bool>> DeleteDesertStormAsync(Guid desertStormId, CancellationToken cancellationToken);
}