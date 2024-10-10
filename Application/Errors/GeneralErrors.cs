namespace Application.Errors;

public static class GeneralErrors
{
    public static readonly Error DatabaseError =
        new("Error.DatabaseError", "An error occurred when saving to the database");
}