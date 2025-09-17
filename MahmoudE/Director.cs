using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFilmStore.Models
{
    public class Director
    {
        public int DirectorId { get; set; }

        [Required]
        public string Name { get; set; }

        // Navigation property
        public List<Film> Films { get; set; } = new List<Film>();
    }
}
