using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFilmStore.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFilmStore.Data;
using EFilmStore.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EFilmStore.Services
{
    public class FilmService : IFilmService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FilmService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Befintliga metoder
        public async Task<List<FilmDto>> GetAllFilms()
        {
            return await _context.Films
                .Include(f => f.Director)
                .ProjectTo<FilmDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<FilmDto> CreateFilm(FilmDto filmDto)
        {
            var film = _mapper.Map<Film>(filmDto);
            _context.Films.Add(film);
            await _context.SaveChangesAsync();
            return _mapper.Map<FilmDto>(film);
        }

        // NY: Filtrering & sortering
        public async Task<List<FilmDto>> GetFilmsFiltered(string? filter, string? sortOrder)
        {
            var query = _context.Films
                .Include(f => f.Director)
                .AsQueryable();

            // Filtrering
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(f => f.Title.Contains(filter) ||
                                         f.Director.Name.Contains(filter));
            }

            // Sortering
            query = (sortOrder?.ToLower() == "desc")
                ? query.OrderByDescending(f => f.Title)
                : query.OrderBy(f => f.Title);

            return await query
                .ProjectTo<FilmDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
