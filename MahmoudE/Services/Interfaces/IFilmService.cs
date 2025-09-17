using EFilmStore.DTOs;

public interface IFilmService
{
    Task<List<FilmDto>> GetAllFilms();
    Task<FilmDto> CreateFilm(FilmDto filmDto);
    Task<List<FilmDto>> GetFilmsFiltered(string? filter, string? sortOrder);
}
