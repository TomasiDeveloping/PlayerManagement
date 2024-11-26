namespace Application.DataTransferObjects.DesertStormParticipants;

public class DesertStormParticipantDto
{
    public Guid Id { get; set; }

    public Guid DesertStormId { get; set; }

    public Guid PlayerId { get; set; }

    public required string PlayerName { get; set; }

    public bool Registered { get; set; }

    public bool Participated { get; set; }

    public bool StartPlayer { get; set; }
}