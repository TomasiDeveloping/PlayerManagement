using System.Text;
using Application.DataTransferObjects.PlayerCombatRecord;
using Application.Interfaces;
using Asp.Versioning;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class PlayerCombatRecordsController(
        IPlayerCombatRecordService service,
        ILogger<PlayerCombatRecordsController> logger) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreatePlayerCombatRecord([FromBody] SubmitPlayerCombatRecordDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await service.SubmitRecordAsync(dto, cancellationToken);
                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(CreatePlayerCombatRecord)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpGet("alliance-combat/{allianceId:guid}")]
        [Authorize]
        public async Task<ActionResult<List<PlayerCombatRecordOverviewDto>>> GetPlayerCombatRecord(Guid allianceId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await service.GetLatestRecordsByAllianceIdAsync(allianceId, cancellationToken);
                if (result.Count == 0)
                {
                    return NoContent();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetPlayerCombatRecord)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpGet("player/{playerId:guid}")]
        public async Task<ActionResult<List<PlayerCombatRecordDto>>> GetRecordsByPlayerId(Guid playerId,[FromQuery] int? limit, CancellationToken cancellationToken)
        {
            try
            {
                var result = await service.GetRecordsByPlayerIdAsync(playerId, limit, cancellationToken);
                if (result.Count == 0)
                {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetRecordsByPlayerId)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpGet("{allianceId:guid}")]
        public async Task<ActionResult<PlayerCombatDto>> GetPlayersByAllianceId(Guid allianceId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await service.GetPlayersByAllianceIdAsync(allianceId, cancellationToken);
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(GetPlayersByAllianceId)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpGet("export/{allianceId:guid}")]
        [Authorize]
        public async Task<IActionResult> ExportAllianceData(Guid allianceId, CancellationToken cancellationToken, [FromQuery] string scope = "latest",
            [FromQuery] string format = "excel")
        {
            try
            {
                var data = await service.GetLatestRecordsByAllianceIdAsync(allianceId, cancellationToken);

                static string GetSquadTypeName(int? type) => type switch
                {
                    0 => "Tank",
                    1 => "Air",
                    2 => "Missile",
                    _ => "-"
                };

                if (format.Equals("csv", StringComparison.CurrentCultureIgnoreCase))
                {
                    var csvBuilder = new StringBuilder();

                    csvBuilder.AppendLine("Player Name,Squad 1 Power (M),Squad 1 Type,Squad 2 Power (M),Squad 2 Type,Squad 3 Power (M),Squad 3 Type,Total Hero Power,Kills (M),Recorded At");

                    foreach (var item in data)
                    {
                        var recordedAtStr = item.RecordedAtUtc.HasValue ? item.RecordedAtUtc.Value.ToString("yyyy-MM-dd HH:mm") : "";
                        csvBuilder.AppendLine($"\"{item.PlayerName}\",{item.Squad1Power},\"{GetSquadTypeName(item.Squad1)}\",{item.Squad2Power},\"{GetSquadTypeName(item.Squad2)}\",{item.Squad3Power},\"{GetSquadTypeName(item.Squad3)}\",{item.TotalHeroPower},{item.Kills},\"{recordedAtStr}\"");
                    }

                    var csvBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());
                    return File(csvBytes, "text/csv", $"alliance-power-{scope}-{DateTime.UtcNow:yyyy-MM-dd}.csv");
                }
                else
                {
                    using var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Alliance Power");

                    var headers = new[] { "Player Name", "Squad 1 Power", "Squad 1 Type", "Squad 2 Power", "Squad 2 Type", "Squad 3 Power", "Squad 3 Type", "Total Hero Power", "Kills", "Recorded At" };

                    for (var i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = headers[i];
                    }

                    var headerRow = worksheet.Row(1);
                    headerRow.Style.Font.Bold = true;
                    headerRow.Style.Font.FontColor = XLColor.White;
                    headerRow.Style.Fill.BackgroundColor = XLColor.FromHtml("#343a40");

                    var rowIdx = 2;
                    foreach (var item in data)
                    {
                        worksheet.Cell(rowIdx, 1).Value = item.PlayerName;
                        worksheet.Cell(rowIdx, 2).Value = item.Squad1Power;
                        worksheet.Cell(rowIdx, 3).Value = GetSquadTypeName(item.Squad1);
                        worksheet.Cell(rowIdx, 4).Value = item.Squad2Power;
                        worksheet.Cell(rowIdx, 5).Value = GetSquadTypeName(item.Squad2);
                        worksheet.Cell(rowIdx, 6).Value = item.Squad3Power;
                        worksheet.Cell(rowIdx, 7).Value = GetSquadTypeName(item.Squad3);
                        worksheet.Cell(rowIdx, 8).Value = item.TotalHeroPower;
                        worksheet.Cell(rowIdx, 9).Value = item.Kills;

                        if (item.RecordedAtUtc.HasValue)
                        {
                            worksheet.Cell(rowIdx, 10).Value = item.RecordedAtUtc.Value.ToLocalTime();
                            worksheet.Cell(rowIdx, 10).Style.DateFormat.Format = "yyyy-MM-dd HH:mm";
                        }
                        else
                        {
                            worksheet.Cell(rowIdx, 10).Value = "-";
                        }

                        rowIdx++;
                    }

                    worksheet.Columns(2, 2).Style.NumberFormat.Format = "0.00\"M\"";
                    worksheet.Columns(4, 4).Style.NumberFormat.Format = "0.00\"M\"";
                    worksheet.Columns(6, 6).Style.NumberFormat.Format = "0.00\"M\"";
                    worksheet.Columns(8, 8).Style.NumberFormat.Format = "#,##0";
                    worksheet.Columns(9, 9).Style.NumberFormat.Format = "0.00\"M\"";

                    worksheet.Columns().AdjustToContents();

                    using var stream = new MemoryStream();
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"alliance-power-{scope}-{DateTime.UtcNow:yyyy-MM-dd}.xlsx"
                    );
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(ExportAllianceData)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<PlayerCombatRecordDto>> UpdateAsync(Guid id, UpdatePlayerCombatDto dto, CancellationToken cancellationToken)
        {
            try
            {
                if (id != dto.Id) return Conflict("The ID in the URL does not match the ID in the request body.");

                var result = await service.UpdateAsync(id, dto, cancellationToken);
                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(UpdateAsync)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<ActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await service.DeleteAsync(id, cancellationToken);
                if (!result)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{ErrorMessage}", e.Message);
                return Problem(
                    detail: $"Failed to process {nameof(DeleteAsync)}",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal server error");
            }
        }
    }
}
