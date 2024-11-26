using Application.Classes;

namespace Application.Interfaces;

public interface IEmailTemplate
{
    public EmailContent ConfirmEmail(string userName, string callBack);

    public EmailContent ResendConfirmationEmail (string userName, string callBack);

    public EmailContent InviteUserEmail(string invitingUserName, string allianceName, string callBack);

    public EmailContent ResetPasswordEmail(string userName, string callBack);
}