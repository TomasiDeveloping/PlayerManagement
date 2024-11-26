using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Note;

public class CreateNoteDto
{
    [Required]
    public Guid PlayerId { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.Now;

    [Required]
    [MaxLength(500)]
    public required string PlayerNote { get; set; }
}