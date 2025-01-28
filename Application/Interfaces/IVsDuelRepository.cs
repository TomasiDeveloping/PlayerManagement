using Application.Classes;
using Application.DataTransferObjects;
using Application.DataTransferObjects.VsDuel;

namespace Application.Interfaces;

public interface IVsDuelRepository
{
    Task<Result<VsDuelDto>> GetVsDuelAsync(Guid vsDuelId, CancellationToken  cancellationToken);

    Task<Result<VsDuelDetailDto>> GetVsDuelDetailAsync(Guid vsDuelId, CancellationToken cancellationToken);

    Task<Result<PagedResponseDto<VsDuelDto>>> GetAllianceVsDuelsAsync(Guid allianceId, int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<Result<VsDuelDto>> CreateVsDuelAsync(CreateVsDuelDto createVsDuelDto, string createdBy, CancellationToken cancellationToken);

    Task<Result<VsDuelDto>> UpdateVsDuelAsync(UpdateVsDuelDto updateVsDuelDto, string modifiedBy, CancellationToken cancellationToken);

    Task<Result<bool>> DeleteVsDuelAsync(Guid vsDuelId, CancellationToken cancellationToken);
}