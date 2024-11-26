namespace Application.Errors;

public class VsDuelParticipantErrors
{
    public static readonly Error NotFound = new("Error.VsDuelParticipant.NotFound",
        "The VsDuelParticipant with the specified identifier was not found");

    public static readonly Error IdConflict = new("Error.VsDuelParticipant.IdConflict", "There is a conflict with the id's");
}