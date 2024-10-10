using Application.Classes;
using Application.DataTransferObjects.Alliance;

namespace Application.Interfaces;

public interface IAllianceRepository
{
    Task<Result<List<AllianceDto>>> GetAlliancesAsync(CancellationToken cancellationToken);

    Task<Result<AllianceDto>> GetAllianceAsync(Guid allianceId, CancellationToken cancellationToken);

    Task<Result<AllianceDto>> CreateAllianceAsync(CreateAllianceDto createAllianceDto, CancellationToken cancellationToken);

    Task<Result<AllianceDto>> UpdateAllianceAsync(UpdateAllianceDto updateAllianceDto, CancellationToken cancellationToken);

    Task<Result<bool>> DeleteAllianceAsync(Guid  allianceId, CancellationToken cancellationToken);
}