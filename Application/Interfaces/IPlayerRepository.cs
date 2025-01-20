using Application.Classes;
using Application.DataTransferObjects.Player;

namespace Application.Interfaces;

public interface IPlayerRepository
{
    Task<Result<PlayerDto>> GetPlayerAsync(Guid playerId, CancellationToken  cancellationToken);

    Task<Result<List<PlayerDto>>> GetAlliancePlayersAsync(Guid allianceId, CancellationToken cancellationToken);

    Task<Result<List<PlayerDto>>> GetAllianceDismissPlayersAsync(Guid allianceId, CancellationToken cancellationToken);

    Task<Result<List<PlayerMvpDto>>> GetAlliancePlayersMvp(Guid allianceId, CancellationToken cancellationToken);

    Task<Result<List<PlayerMvpDto>>> GetAllianceLeadershipMvp(Guid allianceId, CancellationToken cancellationToken);

    Task<Result<DismissPlayerInformationDto>> GetDismissPlayerInformationAsync(Guid playerId, CancellationToken cancellationToken);

    Task<Result<PlayerDto>> CreatePlayerAsync(CreatePlayerDto createPlayerDto, string createdBy, CancellationToken cancellationToken);

    Task<Result<PlayerDto>> UpdatePlayerAsync(UpdatePlayerDto updatePlayerDto, string modifiedBy, CancellationToken cancellationToken);

    Task<Result<PlayerDto>> DismissPlayerAsync(DismissPlayerDto dismissPlayerDto, string modifiedBy, CancellationToken cancellationToken);

    Task<Result<PlayerDto>> ReactivatePlayerAsync(ReactivatePlayerDto reactivatePlayerDto, string modifiedBy, CancellationToken cancellationToken);

    Task<Result<bool>> DeletePlayerAsync(Guid playerIId, CancellationToken cancellationToken);
}