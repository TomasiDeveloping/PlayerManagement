namespace Application.DataTransferObjects.MarshalGuard;

public class MarshalGuardDto
{
    public Guid Id { get; set; }

    public bool Participated { get; set; }

    public int Year { get; set; }

    public int Month { get; set; }

    public int Day { get; set; }
}