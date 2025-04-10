using Application.Classes;
using Application.DataTransferObjects.Note;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Repositories;

public class NoteRepository(ApplicationContext context, IMapper mapper, ILogger<NoteRepository> logger) : INoteRepository
{
    public async Task<Result<NoteDto>> GetNoteAsync(Guid noteId, CancellationToken cancellationToken)
    {
        var noteById = await context.Notes
            .ProjectTo<NoteDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(note => note.Id == noteId, cancellationToken);

        return noteById is null
            ? Result.Failure<NoteDto>(NoteErrors.NotFound)
            : Result.Success(noteById);
    }

    public async Task<Result<List<NoteDto>>> GetPlayerNotesAsync(Guid playerId, CancellationToken cancellationToken)
    {
        var playerNotes = await context.Notes
            .Where(note => note.PlayerId == playerId)
            .ProjectTo<NoteDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .OrderByDescending(note => note.CreatedOn)
            .ToListAsync(cancellationToken);

        return Result.Success(playerNotes);
    }

    public async Task<Result<NoteDto>> CreateNoteAsync(CreateNoteDto createNoteDto, string createdBy, CancellationToken cancellationToken)
    {
        var newNote = mapper.Map<Note>(createNoteDto);
        newNote.CreatedBy = createdBy;

        await context.Notes.AddAsync(newNote, cancellationToken);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<NoteDto>(newNote));
        }
        catch (Exception e)
        {
            logger.LogError(e, "{DateBaseErrorMessage}", e.Message);
            return Result.Failure<NoteDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<NoteDto>> UpdateNoteAsync(UpdateNoteDto updateNoteDto, string modifiedBy, CancellationToken cancellationToken)
    {
        var noteToUpdate = await context.Notes
            .FirstOrDefaultAsync(note => note.Id == updateNoteDto.Id, cancellationToken);

        if (noteToUpdate is null) return Result.Failure<NoteDto>(NoteErrors.NotFound);

        mapper.Map(updateNoteDto, noteToUpdate);
        noteToUpdate.ModifiedBy = modifiedBy;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<NoteDto>(noteToUpdate));
        }
        catch (Exception e)
        {
            logger.LogError(e, "{DateBaseErrorMessage}", e.Message);
            return Result.Failure<NoteDto>(GeneralErrors.DatabaseError);
        }
    }

    public async Task<Result<bool>> DeleteNoteAsync(Guid noteId, CancellationToken cancellationToken)
    {
        var noteToDelete = await context.Notes
            .FirstOrDefaultAsync(note => note.Id == noteId, cancellationToken);

        if (noteToDelete is null) return Result.Failure<bool>(NoteErrors.NotFound);

        context.Notes.Remove(noteToDelete);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(true);
        }
        catch (Exception e)
        {
            logger.LogError(e, "{DateBaseErrorMessage}", e.Message);
            return Result.Failure<bool>(GeneralErrors.DatabaseError);
        }
    }
}