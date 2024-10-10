using Application.Classes;
using Application.DataTransferObjects.Admonition;

namespace Application.Interfaces;

public interface IAdmonitionRepository
{
    Task<Result<List<AdmonitionDto>>> GetAdmonitionsAsync(CancellationToken cancellationToken);

    Task<Result<List<AdmonitionDto>>> GetPlayerAdmonitionsAsync(Guid playerId, CancellationToken cancellationToken);

    Task<Result<AdmonitionDto>> GetAdmonitionAsync(Guid admonitionId, CancellationToken cancellationToken);

    Task<Result<AdmonitionDto>> CreateAdmonitionAsync(CreateAdmonitionDto createAdmonitionDto, CancellationToken cancellationToken);

    Task<Result<AdmonitionDto>> UpdateAdmonitionAsync(UpdateAdmonitionDto updateAdmonitionDto, CancellationToken cancellationToken);

    Task<Result<bool>> DeleteAdmonitionAsync(Guid admonitionId, CancellationToken cancellationToken);
}