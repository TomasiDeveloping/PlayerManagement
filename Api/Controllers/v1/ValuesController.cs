using Api.Helpers;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ValuesController : ControllerBase
    {
        [AllowApiKey]
        [HttpGet]
        public async Task<IActionResult> Test([FromQuery] Guid allianceId, [FromQuery] string? key)
        {
            return Ok(new
            {
                AllianceId = allianceId,
                Key = key
            });
        }
    }
}
