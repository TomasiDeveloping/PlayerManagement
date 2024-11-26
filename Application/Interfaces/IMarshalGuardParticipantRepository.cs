using Application.Classes;
using Application.DataTransferObjects.MarshalGuardParticipant;

namespace Application.Interfaces;

public interface IMarshalGuardParticipantRepository
{
    Task<Result<MarshalGuardParticipantDto>> GetMarshalGuardParticipantAsync(Guid marshalGuardParticipantId,
        CancellationToken cancellationToken);
    Task<Result<bool>> InsertMarshalGuardParticipantAsync(
        List<CreateMarshalGuardParticipantDto> createMarshalGuardParticipantsDto, CancellationToken cancellationToken);

    Task<Result<List<MarshalGuardParticipantDto>>> GetPlayerMarshalParticipantsAsync(Guid playerId, int last, CancellationToken  cancellationToken);

    Task<Result<MarshalGuardParticipantDto>> UpdateMarshalGuardParticipantAsync(
        UpdateMarshalGuardParticipantDto updateMarshalGuardParticipantDto, CancellationToken cancellationToken);
}