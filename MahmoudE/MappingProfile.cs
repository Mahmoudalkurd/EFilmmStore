using System.IO;
using AutoMapper;
using EFilmStore.DTOs;
using EFilmStore.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EFilmStore.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // DTO -> Entity (fanns redan)
            CreateMap<FilmDto, Film>();
            CreateMap<DirectorDto, Director>();

            // Entity -> DTO (behövs för ProjectTo och GET)
            CreateMap<Film, FilmDto>();
            CreateMap<Director, DirectorDto>();
        }
    }
}
