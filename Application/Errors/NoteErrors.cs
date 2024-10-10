namespace Application.Errors;

public static class NoteErrors
{
    public static readonly Error NotFound = new("Error.Note.NotFound",
        "The note with the specified identifier was not found");

    public static readonly Error IdConflict = new("Error.Note.IdConflict", "There is a conflict with the id's");
}