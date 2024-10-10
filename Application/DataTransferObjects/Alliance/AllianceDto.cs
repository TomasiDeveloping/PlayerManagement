namespace Application.DataTransferObjects.Alliance;

public class AllianceDto
{
    public Guid Id { get; set; }

    public int Server { get; set; }

    public required string Name { get; set; }

    public required string Abbreviation { get; set; }
}