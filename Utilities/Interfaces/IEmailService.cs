using Utilities.Classes;

namespace Utilities.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(EmailMessage emailMessage);
}