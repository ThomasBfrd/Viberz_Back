using Viberz.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Viberz
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Bass House" },
                new Genre { Id = 2, Name = "EDM Trap" },
                new Genre { Id = 3, Name = "Dubstep" },
                new Genre { Id = 4, Name = "Drum & Bass" },
                new Genre { Id = 5, Name = "Tech House" },
                new Genre { Id = 6, Name = "Hard Techno" },
                new Genre { Id = 7, Name = "Garage UK" },
                new Genre { Id = 8, Name = "Hyper Techno" },
                new Genre { Id = 9, Name = "Stutter House" }
                );
        }
    }
}
