using Application.Classes;
using Application.DataTransferObjects.DesertStormParticipants;

namespace Application.Interfaces;

public interface IDesertStormParticipantRepository
{
    Task<Result<DesertStormParticipantDto>> GetDesertStormParticipantAsync(Guid desertStormParticipantId,
        CancellationToken cancellationToken);
    Task<Result<bool>> InsertDesertStormParticipantAsync(
        List<CreateDesertStormParticipantDto> createDesertStormParticipants, CancellationToken cancellationToken);

    Task<Result<List<DesertStormParticipantDto>>> GetPlayerDesertStormParticipantsAsync(Guid playerId, int last, CancellationToken cancellationToken);

    Task<Result<DesertStormParticipantDto>> UpdateDesertStormParticipantAsync(
        UpdateDesertStormParticipantDto updateDesertStormParticipantDto, CancellationToken cancellationToken);
}