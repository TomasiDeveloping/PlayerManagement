using Application.Classes;
using Application.Interfaces;
using ExcelDataReader;
using System.Data;
using Application.DataTransferObjects.ExcelImport;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class ExcelService(ApplicationContext context, ILogger<ExcelService> logger) : IExcelService
{
    public async Task<Result<ExcelImportResponse>> ImportPlayersFromExcelAsync(ExcelImportRequest excelImportRequest, string createdBy, CancellationToken cancellationToken)
    {
        try
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            await using var stream = excelImportRequest.ExcelFile.OpenReadStream();
            using var reader = ExcelReaderFactory.CreateReader(stream);

            var result = reader.AsDataSet();

            var dataTable = result.Tables[0];

            if (dataTable.Rows.Count == 0)
            {
                return Result.Failure<ExcelImportResponse>(new Error("","Excel has no data"));
            } 

            var columnMapping = GetColumnMappingFromHeader(dataTable.Rows[0]);

            var requiredHeader = new[] { "Name", "Rank", "Level" };

            foreach (var header in requiredHeader)
            {
                if (!columnMapping.ContainsKey(header))
                {
                    return Result.Failure<ExcelImportResponse>(new Error("", $"Required header “{header}” is missing in the file."));
                }
            }

            var validRanks = await context.Ranks.AsNoTracking().ToListAsync(cancellationToken);
            var alliancePlayers = await context.Players.Where(p => p.AllianceId == excelImportRequest.AllianceId).Select(p => p.PlayerName.Trim().ToLower())
                .ToListAsync(cancellationToken);

            var players = new List<Player>();

            var addCounter = 0;
            var skipCounter = 0;

            for (var row = 1; row < dataTable.Rows.Count; row++)
            {
                var dataRow = dataTable.Rows[row];

                var name = dataRow[columnMapping["Name"]].ToString()?.Trim();
                var level = int.TryParse(dataRow[columnMapping["Level"]].ToString(), out var parsedLevel) ? parsedLevel : 0;
                var rankName = dataRow[columnMapping["Rank"]].ToString()?.Trim() ?? "R1";

                var rank = validRanks.FirstOrDefault(r => string.Equals(r.Name, rankName, StringComparison.CurrentCultureIgnoreCase));

                if (rank is null)
                {
                    return Result.Failure<ExcelImportResponse>(new Error("", $"Invalid rank “{rankName}” in row {row + 1}."));
                }

                if (string.IsNullOrEmpty(name))
                {
                    return Result.Failure<ExcelImportResponse>(new Error("", $"Player name is row {row + 1} is empty"));
                }

                if (alliancePlayers.Contains(name.ToLower().Trim()))
                {
                    skipCounter++;
                    continue;
                }

                players.Add(new Player()
                {
                    CreatedBy = createdBy,
                    PlayerName = name,
                    Level = level,
                    Id = Guid.CreateVersion7(),
                    RankId = rank.Id,
                    CreatedOn = DateTime.Now,
                    AllianceId = excelImportRequest.AllianceId,
                    IsDismissed = false
                });
                addCounter++;
            }

            await context.Players.AddRangeAsync(players, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(new ExcelImportResponse()
            {
                AddSum = addCounter,
                SkipSum = skipCounter
            });
        }
        catch (Exception e)
        {
            logger.LogError(e, "{ExcelErrorMessage}", e.Message);
            return Result.Failure<ExcelImportResponse>(new Error("", ""));
        }
    }

    private static Dictionary<string, int> GetColumnMappingFromHeader(DataRow headerRow)
    {
        var columnMapping = new Dictionary<string, int>();

        for (var col = 0; col < headerRow.Table.Columns.Count; col++)
        {
            var header = headerRow[col].ToString()?.Trim();
            if (!string.IsNullOrEmpty(header))
            {
                columnMapping.TryAdd(header, col);
            }
        }

        return columnMapping;
    }
}