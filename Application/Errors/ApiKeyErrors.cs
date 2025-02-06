namespace Application.Errors;

public static class ApiKeyErrors
{
    public static readonly Error NotFound = new("Error.ApiKey.NotFound",
        "The api key with the specified identifier was not found");

    public static readonly Error NoKeyForAlliance = new("Error.ApiKey.NoKeyForAlliance",
        "The alliance has no api key");

    public static readonly Error IdConflict = new("Error.ApiKey.IdConflict", "There is a conflict with the id's");
}