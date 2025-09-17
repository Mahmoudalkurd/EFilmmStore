using EFilmStore.Models;
using Microsoft.EntityFrameworkCore;

namespace EFilmStore.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Film> Films { get; set; }
        public DbSet<Director> Directors { get; set; }
    }
}
