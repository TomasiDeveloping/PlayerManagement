namespace Database.Entities;

public class SquadType : BaseEntity
{
    public required string TypeName { get; set; }

    public ICollection<Squad> Squads { get; set; } = [];
}