namespace Application.DataTransferObjects.VsDuelLeague;

public class VsDuelLeagueDto
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public int Code { get; set; }
}