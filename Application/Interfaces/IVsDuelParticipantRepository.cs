using Application.Classes;
using Application.DataTransferObjects.VsDuelParticipant;

namespace Application.Interfaces;

public interface IVsDuelParticipantRepository
{
    Task<Result<VsDuelParticipantDto>> UpdateVsDuelParticipant(VsDuelParticipantDto vsDuelParticipantDto, CancellationToken  cancellationToken);
}