using Microsoft.EntityFrameworkCore;
using Viberz.Domain.Entities;

namespace Viberz.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<XpHistory> XpHistories { get; set; }
        public DbSet<XpGrades> XpGrades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Bass House", SpotifyId = "0Y2Dt0Vfg3VSYBKd2BRGyx" },
                new Genre { Id = 2, Name = "EDM Trap", SpotifyId = "4QXVqNH5XVCDAlAS1HocVW" },
                new Genre { Id = 3, Name = "Dubstep", SpotifyId = "6qxaYSiO7LVEJE6dsjC9QU" },
                new Genre { Id = 4, Name = "Drum & Bass", SpotifyId = "0UkGPS5GMhO0QmJx9MvbJm" },
                new Genre { Id = 5, Name = "Tech House", SpotifyId = "2hDT4wx3d3hFNwKMgIKNiB" },
                new Genre { Id = 6, Name = "Hard Techno", SpotifyId = "6LsBdAO09tOMgkt8RJ5ktv" },
                new Genre { Id = 7, Name = "Garage UK", SpotifyId = "1HzqMHV1TMMZjD0POdlww0" },
                new Genre { Id = 8, Name = "Hyper Techno", SpotifyId = "5WhatiRekXXBNRXeO6jXmV" },
                new Genre { Id = 9, Name = "Stutter House", SpotifyId = "08oVyHSMPq79FNRvdxnnPK" }
                );

            modelBuilder.Entity<XpGrades>().HasData(
                new XpGrades { Id = 1, Level = 1, MinXp = 0, GradeName = "Newbie" },
                new XpGrades { Id = 2, Level = 2, MinXp = 50, GradeName = "Beat Beginner" },
                new XpGrades { Id = 3, Level = 3, MinXp = 120, GradeName = "Bass Novice" },
                new XpGrades { Id = 4, Level = 4, MinXp = 250, GradeName = "Stage Starter" },
                new XpGrades { Id = 5, Level = 5, MinXp = 450, GradeName = "Drop Apprentice" },
                new XpGrades { Id = 6, Level = 6, MinXp = 800, GradeName = "Groove Tinkerer" },
                new XpGrades { Id = 7, Level = 7, MinXp = 1300, GradeName = "Crowd Controller" },
                new XpGrades { Id = 8, Level = 8, MinXp = 2100, GradeName = "Drop Disciple" },
                new XpGrades { Id = 9, Level = 9, MinXp = 3200, GradeName = "Festival Vanguard" },
                new XpGrades { Id = 10, Level = 10, MinXp = 4700, GradeName = "Bass Boss" },
                new XpGrades { Id = 11, Level = 11, MinXp = 6700, GradeName = "Sound Sorcerer" },
                new XpGrades { Id = 12, Level = 12, MinXp = 9500, GradeName = "Legendary Master" }
            );

        }
    }
}
