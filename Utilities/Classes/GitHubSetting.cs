using System.ComponentModel.DataAnnotations;

namespace Utilities.Classes;

public class GitHubSetting
{
    [Required]
    public required string Owner { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Token { get; set; }
}