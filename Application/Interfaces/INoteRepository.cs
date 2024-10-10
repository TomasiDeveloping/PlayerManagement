using Application.Classes;
using Application.DataTransferObjects.Note;

namespace Application.Interfaces;

public interface INoteRepository
{
    Task<Result<NoteDto>> GetNoteAsync(Guid noteId, CancellationToken cancellationToken);

    Task<Result<List<NoteDto>>> GetPlayerNotesAsync(Guid playerId, CancellationToken cancellationToken);

    Task<Result<NoteDto>> CreateNoteAsync(CreateNoteDto createNoteDto, CancellationToken cancellationToken);

    Task<Result<NoteDto>> UpdateNoteAsync(UpdateNoteDto updateNoteDto, CancellationToken cancellationToken);

    Task<Result<bool>> DeleteNoteAsync(Guid noteId, CancellationToken cancellationToken);
}