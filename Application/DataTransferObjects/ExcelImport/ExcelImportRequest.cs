using Microsoft.AspNetCore.Http;

namespace Application.DataTransferObjects.ExcelImport;

public class ExcelImportRequest
{
    public required IFormFile ExcelFile { get; set; }

    public Guid AllianceId { get; set; }
}