namespace Application.Errors;

public static class DesertStormErrors
{
    public static readonly Error NotFound = new("Error.DesertStorm.NotFound",
        "The Desert Storm with the specified identifier was not found");

    public static readonly Error IdConflict = new("Error.DesertStorm.IdConflict", "There is a conflict with the id's");
}