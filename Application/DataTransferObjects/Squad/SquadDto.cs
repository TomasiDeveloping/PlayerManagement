namespace Application.DataTransferObjects.Squad;

public class SquadDto
{
    public Guid Id { get; set; }

    public Guid SquadTypeId { get; set; }

    public Guid PlayerId { get; set; }

    public required string TypeName { get; set; }

    public decimal Power { get; set; }

    public int Position { get; set; }

    public DateTime LastUpdateAt { get; set; }

}