using Application.Classes;
using Application.DataTransferObjects.SquadType;

namespace Application.Interfaces;

public interface ISquadTypeRepository
{
    Task<Result<List<SquadTypeDto>>> GetSquadTypesAsync(CancellationToken cancellationToken);
}