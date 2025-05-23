﻿using Application.DataTransferObjects.Note;
using Application.Errors;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class NotesController(INoteRepository noteRepository, IClaimTypeService claimTypeService, ILogger<NotesController> logger) : ControllerBase
    {
        [HttpGet("{noteId:guid}")]
        public async Task<ActionResult<NoteDto>> GetNote(Guid noteId, CancellationToken cancellationToken)
        {
            try
            {
                var noteResult = await noteRepository.GetNoteAsync(noteId, cancellationToken);

                return noteResult.IsFailure
                    ? BadRequest(noteResult.Error)
                    : Ok(noteResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetNote)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpGet("Player/{playerId:guid}")]
        public async Task<ActionResult<List<NoteDto>>> GetPlayerNotes(Guid playerId,
            CancellationToken cancellationToken)
        {
            try
            {
                var playerNotesResult = await noteRepository.GetPlayerNotesAsync(playerId, cancellationToken);

                if (playerNotesResult.IsFailure) return BadRequest(playerNotesResult.Error);

                return playerNotesResult.Value.Count > 0
                    ? Ok(playerNotesResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetPlayerNotes)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<NoteDto>> CreateNote(CreateNoteDto createNoteDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var createResult = await noteRepository.CreateNoteAsync(createNoteDto, claimTypeService.GetFullName(User), cancellationToken);

                return createResult.IsFailure
                    ? BadRequest(createResult.Error)
                    : CreatedAtAction(nameof(GetNote), new { noteId = createResult.Value.Id }, createResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(CreateNote)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpPut("{noteId:guid}")]
        public async Task<ActionResult<NoteDto>> UpdateNote(Guid noteId, UpdateNoteDto updateNoteDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                if (noteId != updateNoteDto.Id) return Conflict(NoteErrors.IdConflict);

                var updateResult = await noteRepository.UpdateNoteAsync(updateNoteDto, claimTypeService.GetFullName(User), cancellationToken);

                return updateResult.IsFailure
                    ? BadRequest(updateResult.Error)
                    : Ok(updateResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(UpdateNote)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpDelete("{noteId:guid}")]
        public async Task<ActionResult<bool>> DeleteNote(Guid noteId, CancellationToken cancellationToken)
        {
            try
            {
                var deleteResult = await noteRepository.DeleteNoteAsync(noteId, cancellationToken);

                return deleteResult.IsFailure
                    ? BadRequest(deleteResult.Error)
                    : Ok(deleteResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(DeleteNote)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }
    }
}
