using Application.Classes;
using Application.DataTransferObjects.Player;

namespace Application.Interfaces;

public interface IPlayerRepository
{
    Task<Result<PlayerDto>> GetPlayerAsync(Guid playerId, CancellationToken  cancellationToken);

    Task<Result<List<PlayerDto>>> GetAlliancePlayersAsync(Guid allianceId, CancellationToken cancellationToken);

    Task<Result<PlayerDto>> CreatePlayerAsync(CreatePlayerDto createPlayerDto, CancellationToken cancellationToken);

    Task<Result<PlayerDto>> UpdatePlayerAsync(UpdatePlayerDto  updatePlayerDto, CancellationToken cancellationToken);

    Task<Result<bool>> DeletePlayerAsync(Guid playerIId, CancellationToken cancellationToken);
}