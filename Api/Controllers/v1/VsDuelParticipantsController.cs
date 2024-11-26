using Application.DataTransferObjects.VsDuelParticipant;
using Application.Errors;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class VsDuelParticipantsController(IVsDuelParticipantRepository vsDuelParticipantRepository, ILogger<VsDuelParticipantsController> logger) : ControllerBase
    {
        [HttpPut("{vsDuelParticipantId:guid}")]
        public async Task<ActionResult<VsDuelParticipantDto>> UpdateVsDuelParticipant(Guid vsDuelParticipantId, VsDuelParticipantDto vsDuelParticipantDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                if (vsDuelParticipantId != vsDuelParticipantDto.Id) return Conflict(VsDuelParticipantErrors.IdConflict);

                var updateResult =
                    await vsDuelParticipantRepository.UpdateVsDuelParticipant(vsDuelParticipantDto, cancellationToken);

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
    }
}
