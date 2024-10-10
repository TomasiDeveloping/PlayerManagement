namespace Application.DataTransferObjects.Admonition;

public class AdmonitionDto
{
    public Guid Id { get; set; }

    public required string Reason { get; set; }

    public Guid PlayerId { get; set; }
}