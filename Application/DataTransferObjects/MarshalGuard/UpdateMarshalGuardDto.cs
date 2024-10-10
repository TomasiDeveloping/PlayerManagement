using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.MarshalGuard;

public class UpdateMarshalGuardDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid PlayerId { get; set; }

    [Required]
    public bool Participated { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    public int Month { get; set; }

    [Required]
    public int Day { get; set; }
}