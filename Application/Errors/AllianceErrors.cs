namespace Application.Errors;

public static class AllianceErrors
{
    public static readonly Error NotFound = new("Error.Alliance.NotFound",
        "The alliance with the specified identifier was not found");

    public static readonly Error IdConflict = new("Error.Alliance.IdConflict", "There is a conflict with the id's");
}