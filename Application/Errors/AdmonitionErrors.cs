namespace Application.Errors;

public static class AdmonitionErrors
{
    public static readonly Error NotFound = new("Error.Admonition.NotFound",
        "The Admonition with the specified identifier was not found");

    public static readonly Error IdConflict = new("Error.Admonition.IdConflict", "There is a conflict with the id's");
}