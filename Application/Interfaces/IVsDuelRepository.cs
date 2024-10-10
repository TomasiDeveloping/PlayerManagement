using Application.Classes;
using Application.DataTransferObjects.VsDuel;

namespace Application.Interfaces;

public interface IVsDuelRepository
{
    Task<Result<VsDuelDto>> GetVsDuelAsync(Guid vsDuelId, CancellationToken  cancellationToken);

    Task<Result<List<VsDuelDto>>> GetPlayerVsDuelsAsync(Guid playerId, CancellationToken cancellationToken);

    Task<Result<VsDuelDto>> CreateVsDuelAsync(CreateVsDuelDto createVsDuelDto, CancellationToken cancellationToken);

    Task<Result<VsDuelDto>> UpdateVsDuelAsync(UpdateVsDuelDto updateVsDuelDto, CancellationToken cancellationToken);

    Task<Result<bool>> DeleteVsDuelAsync(Guid vsDuelId, CancellationToken cancellationToken);
}