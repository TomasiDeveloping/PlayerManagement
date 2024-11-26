namespace Application.Errors;

public class UserErrors
{
    public static readonly Error NotFound = new("Error.User.NotFound",
        "The user with the specified identifier was not found");

    public static readonly Error IdConflict = new("Error.User.IdConflict", "There is a conflict with the id's");

    public static readonly Error CurrentPasswordNotMatch = new("Error.User.CurrentPassword", "The current password is invalid");

    public static readonly Error ChangePasswordFailed = new("Error.User.ChangePasswordFailed", "Password change failed");

    public static readonly Error ConfirmPasswordNotMatch = new("Error.User.ConfirmPasswordNotMatch", "The confirm password not match");
}