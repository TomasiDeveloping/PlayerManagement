namespace Application.Errors;

public static class MarshalGuardErrors
{
    public static readonly Error NotFound = new("Error.MarshalGuard.NotFound",
        "The marshal guard with the specified identifier was not found");

    public static readonly Error IdConflict = new("Error.MarshalGuard.IdConflict", "There is a conflict with the id's");
}