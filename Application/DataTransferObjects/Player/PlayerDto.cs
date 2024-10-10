namespace Application.DataTransferObjects.Player;

public class PlayerDto
{
    public Guid Id { get; set; }

    public required string PlayerName { get; set; }

    public required string Level { get; set; }

    public required string RankName { get; set; }
}