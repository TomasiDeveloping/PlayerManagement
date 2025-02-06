using Api.Helpers;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.WebEncoders.Testing;

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
