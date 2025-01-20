using Application.DataTransferObjects.Admonition;
using Application.DataTransferObjects.Note;

namespace Application.DataTransferObjects.Player;

public class DismissPlayerInformationDto
{
    public Guid Id { get; set; }

    public required string PlayerName { get; set; }

    public DateTime DismissedAt { get; set; }

    public required string DismissalReason { get; set; }

    public ICollection<NoteDto> Notes { get; set; } = [];

    public ICollection<AdmonitionDto> Admonitions { get; set; } = [];

    public ICollection<DismissDesertStormParticipant> DesertStormParticipants { get; set; } = [];

    public ICollection<DismissMarshalParticipant> MarshalGuardParticipants { get; set; } = [];

    public ICollection<DismissVsDuelParticipant> VsDuelParticipants { get; set; } = [];
}

public record DismissDesertStormParticipant(DateTime EventDate, bool Participated);

public record DismissMarshalParticipant(DateTime  EventDate, bool Participated);

public record DismissVsDuelParticipant(DateTime  EventDate, long WeeklyPoints);