namespace Application.DataTransferObjects.Note;

public class NoteDto
{
    public Guid Id { get; set; }

    public required string PlayerNote { get; set; }
}