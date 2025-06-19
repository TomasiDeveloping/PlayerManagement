using Application.Classes;
using Application.DataTransferObjects.Squad;

namespace Application.Interfaces;

public interface ISquadRepository
{
    Task<Result<List<SquadDto>>> GetPlayerSquadsAsync(Guid playerId, CancellationToken cancellationToken = default);

    Task<Result<SquadDto>> CreateSquadAsync(CreateSquadDto createSquadDto, CancellationToken cancellationToken = default);

    Task<Result<SquadDto>> UpdateSquadAsync(UpdateSquadDto updateSquadDto, CancellationToken cancellationToken = default);

    Task<Result<bool>> DeleteSquadAsync(Guid squadId, CancellationToken cancellationToken = default);
}