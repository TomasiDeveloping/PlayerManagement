using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Note;

public class CreateNoteDto
{
    [Required]
    public Guid PlayerId { get; set; }

    [Required]
    [MaxLength(500)]
    public required string PlayerNote { get; set; }
}