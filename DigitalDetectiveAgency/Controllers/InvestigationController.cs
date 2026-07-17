using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalDetectiveAgency.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InvestigationController : ControllerBase
    {
        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok(new { system = "Investigation processing engine fully online." });
        }
    }
}