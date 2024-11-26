namespace Application.DataTransferObjects.Note;

public class NoteDto
{
    public Guid Id { get; set; }

    public Guid PlayerId { get; set; }

    public DateTime CreatedOn { get; set; }

    public required string CreatedBy { get; set; }

    public required string PlayerNote { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
}