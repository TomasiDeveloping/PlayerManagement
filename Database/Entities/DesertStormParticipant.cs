namespace Database.Entities;

public class DesertStormParticipant : BaseEntity
{
    public Player Player { get; set; } = null!;

    public Guid PlayerId { get; set; }

    public DesertStorm DesertStorm { get; set; } = null!;

    public Guid DesertStormId { get; set; }

    public bool Registered { get; set; }

    public bool Participated { get; set; }

    public bool StartPlayer { get; set; }
}