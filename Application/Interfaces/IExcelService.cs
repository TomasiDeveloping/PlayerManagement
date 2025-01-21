using Application.Classes;
using Application.DataTransferObjects.ExcelImport;

namespace Application.Interfaces;

public interface IExcelService
{
    Task<Result<ExcelImportResponse>> ImportPlayersFromExcelAsync(ExcelImportRequest excelImportRequest, string createdBy, CancellationToken cancellationToken);
}