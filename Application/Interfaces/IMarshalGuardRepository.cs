using Application.Classes;
using Application.DataTransferObjects.MarshalGuard;

namespace Application.Interfaces;

public interface IMarshalGuardRepository
{
    Task<Result<MarshalGuardDto>> GetMarshalGuardAsync(Guid marshalGuardId, CancellationToken cancellationToken);

    Task<Result<List<MarshalGuardDto>>> GetPlayerMarshalGuardsAsync(Guid playerId, CancellationToken cancellationToken);

    Task<Result<MarshalGuardDto>> CreateMarshalGuardAsync(CreateMarshalGuardDto createMarshalGuardDto, CancellationToken cancellationToken);

    Task<Result<MarshalGuardDto>> UpdateMarshalGuardAsync(UpdateMarshalGuardDto updateMarshalGuardDto, CancellationToken cancellationToken);

    Task<Result<bool>> DeleteMarshalGuardAsync(Guid marshalGuardId, CancellationToken cancellationToken);
}