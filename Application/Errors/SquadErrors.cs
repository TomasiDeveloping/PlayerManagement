namespace Application.Errors;

public class SquadErrors
{
    public static readonly Error NotFound = new("Error.Squad.NotFound",
        "The quad with the specified identifier was not found");

    public static readonly Error IdConflict = new("Error.Note.IdConflict", "There is a conflict with the id's");
}