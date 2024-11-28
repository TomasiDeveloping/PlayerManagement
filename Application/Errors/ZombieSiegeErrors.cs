namespace Application.Errors;

public static class ZombieSiegeErrors
{
    public static readonly Error NotFound = new("Error.ZombieSiege.NotFound",
        "The zombie siege with the specified identifier was not found");

    public static readonly Error IdConflict = new("Error.ZombieSiege.IdConflict", "There is a conflict with the id's");
}