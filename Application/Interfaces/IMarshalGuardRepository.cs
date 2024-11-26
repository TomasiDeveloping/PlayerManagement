using Application.Classes;
using Application.DataTransferObjects.MarshalGuard;

namespace Application.Interfaces;

public interface IMarshalGuardRepository
{
    Task<Result<MarshalGuardDto>> GetMarshalGuardAsync(Guid marshalGuardId, CancellationToken cancellationToken);

    Task<Result<MarshalGuardDetailDto>> GetMarshalGuardDetailAsync(Guid marshalGuardId, CancellationToken cancellationToken);

    Task<Result<List<MarshalGuardDto>>> GetAllianceMarshalGuardsAsync(Guid allianceId, int take, CancellationToken cancellationToken);

    Task<Result<MarshalGuardDto>> CreateMarshalGuardsAsync(CreateMarshalGuardDto createMarshalGuardDto, string createdBy, CancellationToken cancellationToken);

    Task<Result<MarshalGuardDto>> UpdateMarshalGuardAsync(UpdateMarshalGuardDto updateMarshalGuardDto, string modifiedBy, CancellationToken cancellationToken);

    Task<Result<bool>> DeleteMarshalGuardAsync(Guid marshalGuardId, CancellationToken cancellationToken);
}