using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.MarshalGuard;

public class CreateMarshalGuardDto
{
    [Required]
    public Guid PlayerId { get; set; }

    [Required]
    public bool Participated { get; set; }

}