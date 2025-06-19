namespace Application.DataTransferObjects.Squad;

public class CreateSquadDto
{
    public Guid SquadTypeId { get; set; }

    public Guid PlayerId { get; set; }

    public decimal Power { get; set; }

    public int Position { get; set; }
}