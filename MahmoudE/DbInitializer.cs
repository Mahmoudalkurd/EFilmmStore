using System;
using EFilmStore.Models;

namespace EFilmStore.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // Om det redan finns filmer -> gör inget
            if (context.Films.Any()) return;

            // 1) Lägg till regissörer först och spara för att få riktiga ID:n
            var directors = new List<Director>
            {
                new() { Name = "Christopher Nolan" },
                new() { Name = "Steven Spielberg" }
            };

            context.Directors.AddRange(directors);
            context.SaveChanges(); // Nu har directors[0].DirectorId och directors[1].DirectorId värden

            // 2) Skapa filmer och koppla antingen via DirectorId eller navigation
            // Rekommenderat: koppla via navigation (mindre risk för fel)
            var films = new List<Film>
            {
                new Film { Title = "Inception", Price = 149, Director = directors[0] },
                new Film { Title = "Jurassic Park", Price = 129, Director = directors[1] }
            };

            context.Films.AddRange(films);
            context.SaveChanges();
        }
    }
}
