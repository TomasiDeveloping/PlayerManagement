using Microsoft.AspNetCore.Http;
using MimeKit;

namespace Utilities.Classes;

public class EmailMessage
{
    public EmailMessage(IEnumerable<string> to, string subject, string content, IFormFileCollection? attachments = null)
    {
        To = [];
        To.AddRange(to.Select(mail => new MailboxAddress(name: mail, address: mail)));
        Subject = subject;
        Content = content;
        Attachments = attachments;
    }

    public List<MailboxAddress> To { get; set; }

    public string Subject { get; set; }

    public string Content { get; set; }

    public IFormFileCollection? Attachments { get; set; }
}