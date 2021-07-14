using Microsoft.AspNetCore.Mvc;

namespace mars_rover.Controllers
{
    public class HealthController : BaseController
    {
        [HttpGet]
        public IActionResult Get() => new OkObjectResult(new { status = "wearegoodtogo" });
    }
}
