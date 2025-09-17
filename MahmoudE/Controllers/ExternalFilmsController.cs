using Microsoft.AspNetCore.Mvc;
using EFilmStore.Services;

namespace EFilmStore.Controllers
{
    [ApiController]
    [Route("api/external-films")]
    public class ExternalFilmsController : ControllerBase
    {
        private readonly ExternalFilmService _externalFilmService;

        public ExternalFilmsController(ExternalFilmService externalFilmService)
        {
            _externalFilmService = externalFilmService;
        }

        [HttpGet]
        public async Task<IActionResult> GetExternalFilms([FromQuery] string query = null)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    return BadRequest("Please provide a search query");
                }

                var films = await _externalFilmService.SearchFilmsAsync(query);
                return Ok(films);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
