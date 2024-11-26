using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Utilities.Classes;
using Utilities.Interfaces;

namespace Utilities.Services;

public class EmailService(IOptions<EmailConfiguration> emailConfigurationOptions, ILogger<EmailService> logger)
    : IEmailService
{
    private readonly EmailConfiguration _emailConfiguration = emailConfigurationOptions.Value;

    public async Task<bool> SendEmailAsync(EmailMessage emailMessage)
    {
        var mimeMessage = await CreateMailMessage(emailMessage);
        return await SendAsync(mimeMessage);
    }

    private async Task<bool> SendAsync(MimeMessage mimeMessage)
    {
        using var client = new SmtpClient();

        try
        {
            await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);

            client.AuthenticationMechanisms.Remove("XOAUTH2");

            await client.AuthenticateAsync(_emailConfiguration.UserName, _emailConfiguration.Password);

            await client.SendAsync(mimeMessage);

            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return false;
        }
    }

    private async Task<MimeMessage> CreateMailMessage(EmailMessage emailMessage)
    {
        var mimeMessage = new MimeMessage();

        mimeMessage.From.Add(new MailboxAddress(_emailConfiguration.DisplayName, _emailConfiguration.FromAddress));

        mimeMessage.To.AddRange(emailMessage.To);

        mimeMessage.Subject = emailMessage.Subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = emailMessage.Content
        };

        if (emailMessage.Attachments is not null && emailMessage.Attachments.Any())
        {
            foreach (var attachment in emailMessage.Attachments)
            {
                using var memoryStream = new MemoryStream();
                await attachment.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();

                bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
            }
        }
        
        mimeMessage.Body = bodyBuilder.ToMessageBody();
        return mimeMessage;
    }
}