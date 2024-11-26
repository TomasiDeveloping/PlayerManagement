namespace Application.Errors;

public static class CustomEventErrors
{
    public static readonly Error NotFound = new("Error.CustomEvent.NotFound",
        "The custom event with the specified identifier was not found");

    public static readonly Error IdConflict = new("Error.CustomEvent.IdConflict", "There is a conflict with the id's");
}