using Swashbuckle.AspNetCore.Annotations;

namespace Application.DataTransferObjects.Player;

public class PlayerMvpDto
{
    [SwaggerSchema(Description = "Name of the player.")]
    public required string Name { get; set; }

    [SwaggerSchema(Description = "The alliance rank of the player, which can be R5, R4, R3, R2, or R1.")]
    public required string AllianceRank { get; set; }

    [SwaggerSchema(Description = "Total points the player earned in the last three weeks based on duel performance.")]
    public long DuelPointsLast3Weeks { get; set; }

    [SwaggerSchema(Description = "The number of times the player participated in the Marshal competition in the last three weeks.")]
    public int MarshalParticipationCount { get; set; }

    [SwaggerSchema(Description = "The number of times the player participated in the Desert Storm competition in the last three weeks.")]
    public int DesertStormParticipationCount { get; set; }

    [SwaggerSchema(Description = "Indicates whether the player has participated in the oldest VS Duel for at least the last three weeks.")]
    public bool HasParticipatedInOldestDuel { get; set; }

    [SwaggerSchema(Description = "The total points earned by the player according to the MVP formula.")]
    public decimal MvpScore { get; set; }
}