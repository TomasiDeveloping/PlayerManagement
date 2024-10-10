namespace Application.Errors;

public class VsDuelErrors
{
    public static readonly Error NotFound = new("Error.VsDuel.NotFound",
        "The Vs Duel with the specified identifier was not found");

    public static readonly Error IdConflict = new("Error.VsDuel.IdConflict", "There is a conflict with the id's");
}