using System.Runtime.InteropServices.JavaScript;

namespace Application.DataTransferObjects.Player;

public class PlayerDto
{
    public Guid Id { get; set; }

    public Guid RankId { get; set; }

    public Guid AllianceId { get; set; }

    public required string PlayerName { get; set; }

    public int Level { get; set; }

    public required string RankName { get; set; }

    public int NotesCount { get; set; }

    public int AdmonitionsCount { get; set; }

    public DateTime CreatedOn { get; set; }

    public required string CreatedBy { get; set; }

    public DateTime? ModifiedOn  { get; set; }

    public string? ModifiedBy { get; set; }
}