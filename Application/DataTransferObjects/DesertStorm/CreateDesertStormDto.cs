namespace Application.DataTransferObjects.DesertStorm;

public class CreateDesertStormDto
{
    public Guid PlayerId { get; set; }

    public bool Registered { get; set; }

    public bool Participated { get; set; }
}