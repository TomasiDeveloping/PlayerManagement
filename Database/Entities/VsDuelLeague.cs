namespace Database.Entities;

public class VsDuelLeague : BaseEntity
{
    public required string Name { get; set; }

    public int Code { get; set; }

    public ICollection<VsDuel> VsDuels { get; set; } = [];
}