using System.IO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFilmStore.Data;
using EFilmStore.DTOs;
using EFilmStore.Models;
using Microsoft.EntityFrameworkCore;

namespace EFilmStore.Services
{
    public class DirectorService : IDirectorService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public DirectorService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DirectorDto>> GetAllDirectors()
        {
            return await _context.Directors
                .AsNoTracking()
                .ProjectTo<DirectorDto>(_mapper.ConfigurationProvider) // Använd AutoMappers ProjectTo
                .ToListAsync();
        }

        public async Task<DirectorDto> CreateDirector(DirectorDto directorDto)
        {
            var director = _mapper.Map<Director>(directorDto);
            _context.Directors.Add(director);
            await _context.SaveChangesAsync();
            return _mapper.Map<DirectorDto>(director);
        }
    }
}
