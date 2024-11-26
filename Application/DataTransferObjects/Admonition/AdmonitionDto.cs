namespace Application.DataTransferObjects.Admonition;

public class AdmonitionDto
{
    public Guid Id { get; set; }

    public required string Reason { get; set; }

    public DateTime CreatedOn { get; set; }

    public required string CreatedBy { get; set; }

    public Guid PlayerId { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
}