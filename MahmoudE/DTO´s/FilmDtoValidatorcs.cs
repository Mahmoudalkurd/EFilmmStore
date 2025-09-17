using EFilmStore.DTOs;
using FluentValidation;

namespace EFilmStore.Validators
{
    public class FilmDtoValidator : AbstractValidator<FilmDto>
    {
        public FilmDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().Length(2, 100);
            RuleFor(x => x.Price).GreaterThan(0);
        }
    }
}
