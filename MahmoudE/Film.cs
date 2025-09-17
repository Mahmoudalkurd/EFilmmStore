using System.IO;

namespace EFilmStore.Models
{
    public class Film
    {
        public int FilmId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int DirectorId { get; set; }  // Se till att detta finns
        public Director Director { get; set; }
    }
}
