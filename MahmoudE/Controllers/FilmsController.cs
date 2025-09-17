using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFilmStore.Data;
using EFilmStore.DTOs;
using EFilmStore.Models;
using EFilmStore.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFilmStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilmsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FilmsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET /api/films?filter=&sortOrder=asc|desc
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FilmDto>>> GetAll([FromQuery] string? filter, [FromQuery] string? sortOrder = "asc")
        {
            var query = _context.Films
                .Include(f => f.Director) // behövs om du mappar directorfält senare
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(f => f.Title.Contains(filter) ||
                                         f.Director.Name.Contains(filter));
            }

            query = (sortOrder?.ToLower() == "desc")
                ? query.OrderByDescending(f => f.Title)
                : query.OrderBy(f => f.Title);

            var result = await query
                .ProjectTo<FilmDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(result);
        }

        // GET /api/films/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<FilmDto>> GetById(int id)
        {
            var film = await _context.Films
                .AsNoTracking()
                .Where(f => f.FilmId == id)
                .ProjectTo<FilmDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return film is null ? NotFound() : Ok(film);
        }

        // POST /api/films
        [HttpPost]
        public async Task<ActionResult<FilmDto>> Create([FromBody] FilmDto filmDto)
        {
            var validator = new FilmDtoValidator();
            var validationResult = await validator.ValidateAsync(filmDto);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            var directorExists = await _context.Directors.AnyAsync(d => d.DirectorId == filmDto.DirectorId);
            if (!directorExists) return BadRequest("DirectorId does not exist.");

            var film = _mapper.Map<Film>(filmDto);
            _context.Films.Add(film);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.GetBaseException().Message}");
            }

            var result = _mapper.Map<FilmDto>(film);
            return CreatedAtAction(nameof(GetById), new { id = film.FilmId }, result);
        }
    }
}

