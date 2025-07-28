using LearnGenres.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearnGenres
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Playlist> Playlists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relation Playlist <-> User (propriétaire)
            modelBuilder.Entity<Playlist>()
                .HasOne(p => p.User)
                .WithMany(u => u.Playlists)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relation Playlist <-> User (likes)
            modelBuilder.Entity<Playlist>()
                .HasMany(p => p.LikedByUsers)
                .WithMany(u => u.PlaylistsLiked);
        }
    }
}
