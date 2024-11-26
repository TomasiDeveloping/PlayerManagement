namespace Application.Errors;

public static class AuthenticationErrors
{
    public static readonly Error LoginFailed = new("Error.Authentication.LoginFailed", "Email or password incorrect");

    public static readonly Error EmailNotConfirmed =
        new("Error.Authentication.EmailNotConfirmed", "Email is not confirmed");

    public static readonly Error RegisterFailed =
        new("Error.Authentication.RegisterFailed", "Could not create an account");

    public static readonly Error AllianceAlreadyExists =
        new("Error.Authentication.AllianceAlreadyExists", "Alliance already exists");

    public static readonly Error ResendConfirmationEmailFailed = new("Error.Authentication.ResendConfirmEmailFailed",
        "Error while resend the email confirmation email");

    public static readonly Error InviteUserFailed = new("Error.Authentication.InviteUser",
        "Error while send email for inviting");
}