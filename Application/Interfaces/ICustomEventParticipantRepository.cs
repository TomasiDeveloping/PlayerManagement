using Application.Classes;
using Application.DataTransferObjects.CustomEventParticipant;

namespace Application.Interfaces;

public interface ICustomEventParticipantRepository
{
    Task<Result<CustomEventParticipantDto>> GetCustomEventParticipantAsync(Guid customEventParticipantId,
        CancellationToken cancellationToken);
    Task<Result<bool>> InsertCustomEventParticipantAsync(
        List<CreateCustomEventParticipantDto> createCustomEventParticipants, CancellationToken cancellationToken);

    Task<Result<List<CustomEventParticipantDto>>> GetPlayerCustomEventParticipantsAsync(Guid playerId, int last, CancellationToken cancellationToken);

    Task<Result<CustomEventParticipantDto>> UpdateCustomEventParticipantAsync(
        UpdateCustomEventParticipantDto updateCustomEventParticipant, CancellationToken cancellationToken);
}