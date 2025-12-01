using Microsoft.AspNetCore.Mvc;

namespace Viberz.API.Controllers;

[Route("api/health")]
[ApiController]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult GetHealthStatus()
    {
        return Ok("API is healthy");
    }
}
