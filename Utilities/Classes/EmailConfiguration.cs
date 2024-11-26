using System.ComponentModel.DataAnnotations;

namespace Utilities.Classes;

public class EmailConfiguration
{
    [Required] public required string FromAddress { get; set; }

    [Required] public required string DisplayName { get; set; }

    [Required] public required string SmtpServer { get; set; }

    [Range(1, 65535)] public int Port { get; set; }

    [Required] public required string UserName { get; set; }

    [Required] public required string Password { get; set; }
}