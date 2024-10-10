namespace Application.Errors;

public static class AuthenticationErrors
{
    public static readonly Error LoginFailed = new("Error.Authentication.LoginFailed", "Email or password incorrect");

    public static readonly Error RegisterFailed =
        new("Error.Authentication.RegisterFailed", "Could not create an account");

    public static readonly Error AllianceAlreadyExists =
        new("Error.Authentication.AllianceAlreadyExists", "Alliance already exists");
}