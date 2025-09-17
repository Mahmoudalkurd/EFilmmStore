using EFilmStore.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFilmStore.Services
{
    public interface IDirectorService
    {
        Task<List<DirectorDto>> GetAllDirectors();
        Task<DirectorDto> CreateDirector(DirectorDto directorDto);
    }
}
