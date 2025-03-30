using Microsoft.EntityFrameworkCore;

namespace alternatrr.Data.Radarr
{
    public partial class RadarrDbContext : DbContext
    {
        public RadarrDbContext(DbContextOptions<RadarrDbContext> options) : base(options)
        {
        }

        public virtual DbSet<SceneMapping> SceneMappings { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SceneMapping>(entity =>
            {
                entity.Property(e => e.ParseTerm).IsRequired();

                entity.Property(e => e.SearchTerm).IsRequired();
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasIndex(e => e.CleanTitle, "IX_Movies_CleanTitle");

                entity.HasIndex(e => e.Path, "IX_Movies_Path");

                entity.HasIndex(e => e.TitleSlug, "IX_Movies_TitleSlug")
                    .IsUnique();

                entity.HasIndex(e => e.ImdbId, "IX_Movies_ImdbId");

                entity.HasIndex(e => e.TmdbId, "IX_Movies_TmdbId")
                    .IsUnique();

                entity.Property(e => e.Added).HasColumnType("DATETIME");

                entity.Property(e => e.CleanTitle).IsRequired();

                entity.Property(e => e.LastDiskSync).HasColumnType("DATETIME");

                entity.Property(e => e.LastInfoSync).HasColumnType("DATETIME");

                entity.Property(e => e.Images).IsRequired();

                entity.Property(e => e.Path).IsRequired();

                entity.Property(e => e.Title).IsRequired();
            });
        }
    }
}
