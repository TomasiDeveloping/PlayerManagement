using Api.Helpers;
using Application.DataTransferObjects;
using Application.DataTransferObjects.ExcelImport;
using Application.DataTransferObjects.Player;
using Application.Errors;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


namespace Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class PlayersController(IPlayerRepository playerRepository, IClaimTypeService claimTypeService, IExcelService excelService, ILogger<PlayersController> logger) : ControllerBase
    {
        [Authorize]
        [HttpGet("{playerId:guid}")]
        public async Task<ActionResult<PlayerDto>> GetPlayer(Guid playerId, CancellationToken cancellationToken)
        {
            try
            {
                var playerResult = await playerRepository.GetPlayerAsync(playerId, cancellationToken);

                return playerResult.IsFailure
                    ? BadRequest(playerResult.Error)
                    : Ok(playerResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpGet("Alliance/{allianceId:guid}")]
        public async Task<ActionResult<List<PlayerDto>>> GetAlliancePlayers(Guid allianceId, CancellationToken cancellationToken)
        {
            try
            {
                var alliancePlayersResult =
                    await playerRepository.GetAlliancePlayersAsync(allianceId, cancellationToken);

                if (alliancePlayersResult.IsFailure) return BadRequest(alliancePlayersResult.Error);

                return alliancePlayersResult.Value.Count > 0
                    ? Ok(alliancePlayersResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpGet("Alliance/dismiss/{allianceId:guid}")]
        public async Task<ActionResult<PagedResponseDto<PlayerDto>>> GetAllianceDismissPlayers(Guid allianceId, CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var allianceDismissPlayersResult =
                    await playerRepository.GetAllianceDismissPlayersAsync(allianceId, pageNumber, pageSize, cancellationToken);

                if (allianceDismissPlayersResult.IsFailure) return BadRequest(allianceDismissPlayersResult.Error);

                return allianceDismissPlayersResult.Value.Data.Count > 0
                    ? Ok(allianceDismissPlayersResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpGet("DismissInformation/{playerId:guid}")]
        public async Task<ActionResult<DismissPlayerInformationDto>> GetDismissPlayerInformation(Guid playerId, CancellationToken cancellationToken)
        {
            try
            {
                var playerDismissInformationResult =
                    await playerRepository.GetDismissPlayerInformationAsync(playerId, cancellationToken);

                return playerDismissInformationResult.IsFailure
                    ? BadRequest(playerDismissInformationResult.Error)
                    : Ok(playerDismissInformationResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AllowAnonymous]
        [HttpGet("[action]/{allianceId:guid}")]
        public async Task<ActionResult<List<PlayerMvpDto>>> GetAllianceMvpPlayers(Guid allianceId,
            CancellationToken cancellationToken)
        {
            try
            {
                var alliancePlayersResult =
                    await playerRepository.GetAlliancePlayersMvp(allianceId, cancellationToken);

                if (alliancePlayersResult.IsFailure) return BadRequest(alliancePlayersResult.Error);

                return alliancePlayersResult.Value.Count > 0
                    ? Ok(alliancePlayersResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AllowAnonymous]
        [HttpGet("[action]/{allianceId:guid}")]
        public async Task<ActionResult<List<PlayerMvpDto>>> GetAllianceLeadershipMvp(Guid allianceId,
            CancellationToken cancellationToken)
        {
            try
            {
                var alliancePlayersResult =
                    await playerRepository.GetAllianceLeadershipMvp(allianceId, cancellationToken);

                if (alliancePlayersResult.IsFailure) return BadRequest(alliancePlayersResult.Error);

                return alliancePlayersResult.Value.Count > 0
                    ? Ok(alliancePlayersResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AllowApiKey]
        [HttpGet("Mvp/")]
        [SwaggerOperation(
            Summary = "Get Alliance MVPs",
            Description = "Retrieves the MVPs (Most Valuable Players) for a given alliance. If the 'playerType' parameter is not provided, all MVPs will be returned. The 'playerType' parameter specifies whether the MVPs should be from 'players' or 'leadership'. Possible values for 'playerType' are 'players' or 'leadership'."
        )]
        public async Task<ActionResult<List<PlayerMvpDto>>> GetAllianceMvp([FromQuery] Guid allianceId, [FromQuery]string? playerType, [FromQuery]string? key, CancellationToken cancellationToken)
        {
            try
            {
                var allianceMvpResult = await playerRepository.GetAllianceMvp(allianceId, playerType, cancellationToken);

                if (allianceMvpResult.IsFailure) return BadRequest(allianceMvpResult.Error);

                return allianceMvpResult.Value.Count > 0
                    ? Ok(allianceMvpResult.Value)
                    : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<PlayerDto>> CreatePlayer(CreatePlayerDto createPlayerDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var createResult = await playerRepository.CreatePlayerAsync(createPlayerDto, claimTypeService.GetFullName(User), cancellationToken);

                return createResult.IsFailure
                    ? BadRequest(createResult.Error)
                    : CreatedAtAction(nameof(GetPlayer), new { playerId = createResult.Value.Id }, createResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpPost("ExcelImport")]
        public async Task<ActionResult<ExcelImportResponse>> ImportPlayersFromExcel(
            [FromForm] ExcelImportRequest excelImportRequest, CancellationToken cancellationToken)
        {
            try
            {
                if (excelImportRequest.ExcelFile.Length == 0)
                {
                    return BadRequest(new Error("", "No excel file upload"));
                }

                var allowedExtensions = new[] { ".xlsx", ".xls" };
                var fileExtension = Path.GetExtension(excelImportRequest.ExcelFile.FileName);

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(new Error("", "No supported excel file"));
                }

                var excelImportResult = await excelService.ImportPlayersFromExcelAsync(excelImportRequest,
                    claimTypeService.GetFullName(User), cancellationToken);

                return excelImportResult.IsFailure
                    ? BadRequest(excelImportResult.Error)
                    : Ok(excelImportResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpPut("{playerId:guid}")]
        public async Task<ActionResult<PlayerDto>> UpdatePlayer(Guid playerId, UpdatePlayerDto updatePlayerDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                if (playerId != updatePlayerDto.Id) return Conflict(PlayerErrors.IdConflict);

                var updateResult = await playerRepository.UpdatePlayerAsync(updatePlayerDto, claimTypeService.GetFullName(User), cancellationToken);

                return updateResult.IsFailure
                    ? BadRequest(updateResult.Error)
                    : Ok(updateResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpPut("{playerId:guid}/dismiss")]
        public async Task<ActionResult<PlayerDto>> DismissPlayer(Guid playerId, DismissPlayerDto dismissPlayerDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                if (playerId != dismissPlayerDto.Id) return Conflict(PlayerErrors.IdConflict);

                var dismissResult = await playerRepository.DismissPlayerAsync(dismissPlayerDto, claimTypeService.GetFullName(User), cancellationToken);

                return dismissResult.IsFailure
                    ? BadRequest(dismissResult.Error)
                    : Ok(dismissResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpPut("{playerId:guid}/reactive")]
        public async Task<ActionResult<PlayerDto>> ReactivePlayer(Guid playerId, ReactivatePlayerDto reactivatePlayerDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                if (playerId != reactivatePlayerDto.Id) return Conflict(PlayerErrors.IdConflict);

                var reactiveResult = await playerRepository.ReactivatePlayerAsync(reactivatePlayerDto, claimTypeService.GetFullName(User), cancellationToken);

                return reactiveResult.IsFailure
                    ? BadRequest(reactiveResult.Error)
                    : Ok(reactiveResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpDelete("{playerId:guid}")]
        public async Task<ActionResult<bool>> DeletePlayer(Guid playerId, CancellationToken cancellationToken)
        {
            try
            {
                var deleteResult = await playerRepository.DeletePlayerAsync(playerId, cancellationToken);

                return deleteResult.IsFailure
                    ? BadRequest(deleteResult.Error)
                    : Ok(deleteResult.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
