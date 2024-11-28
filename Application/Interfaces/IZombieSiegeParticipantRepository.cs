using Application.Classes;
using Application.DataTransferObjects.ZombieSiegeParticipant;

namespace Application.Interfaces;

public interface IZombieSiegeParticipantRepository
{
    Task<Result<ZombieSiegeParticipantDto>> GetZombieSiegeParticipantAsync(Guid zombieSiegeParticipantId, CancellationToken cancellationToken);

    Task<Result<bool>> InsertZombieSiegeParticipantsAsync(List<CreateZombieSiegeParticipantDto> createZombieSiegeParticipants, CancellationToken cancellationToken);

    Task<Result<List<ZombieSiegeParticipantDto>>> GetPlayerZombieSiegeParticipantsAsync(Guid playerId, int last, CancellationToken cancellationToken);

    Task<Result<ZombieSiegeParticipantDto>> UpdateZombieSiegeParticipantAsync(UpdateZombieSiegeParticipantDto updateZombieSiegeParticipantDto, CancellationToken cancellationToken);
}