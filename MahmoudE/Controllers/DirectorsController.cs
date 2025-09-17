using System.IO;
using AutoMapper;
using EFilmStore.Data;
using EFilmStore.DTOs;
using EFilmStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFilmStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectorsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public DirectorsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Director>> CreateDirector(DirectorDto directorDto)
        {
            try
            {
                var director = _mapper.Map<Director>(directorDto);
                _context.Directors.Add(director);
                await _context.SaveChangesAsync();
                return Ok(director);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Director>>> GetAllDirectors()
        {
            try
            {
                var directors = await _context.Directors.ToListAsync();
                return Ok(directors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
