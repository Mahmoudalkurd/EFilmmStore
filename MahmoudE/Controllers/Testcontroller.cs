using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFilmStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [Authorize]
        [HttpGet("secure")]
        public IActionResult GetSecureData()
        {
            return Ok("This is protected data only accessible with a valid JWT token.");
        }

        [AllowAnonymous]
        [HttpGet("public")]
        public IActionResult GetPublicData()
        {
            return Ok("This is public data.");
        }
    }
}
