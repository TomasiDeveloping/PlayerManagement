namespace Application.Errors;

public static class RoleErrors
{
    public static readonly Error NotFound = new("Error.Role.NotFound",
        "The role with the specified identifier was not found");
}