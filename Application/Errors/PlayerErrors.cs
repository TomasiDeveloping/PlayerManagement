namespace Application.Errors;

public static class PlayerErrors
{
    public static readonly Error NotFound = new("Error.Player.NotFound",
        "The player with the specified identifier was not found");

    public static readonly Error IdConflict = new("Error.Player.IdConflict", "There is a conflict with the id's");
}