using Microsoft.EntityFrameworkCore;

namespace alternatrr.Data.Radarr
{
    public partial class RadarrDbContext : DbContext
    {
        public RadarrDbContext(DbContextOptions<RadarrDbContext> options) : base(options)
        {
        }

        public virtual DbSet<AlternativeTitle> AlternativeTitles { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<MovieMetadata> MovieMetadata { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlternativeTitle>(entity =>
            {
                entity.HasIndex(e => e.CleanTitle, "IX_AlternativeTitles_CleanTitle");

                entity.HasIndex(e => e.MovieMetadataId, "IX_AlternativeTitles_MovieMetadataId");

                entity.Property(e => e.CleanTitle).IsRequired();

                entity.Property(e => e.Title).IsRequired();

                entity.HasOne(d => d.MovieMetadata)
                    .WithMany(p => p.AlternativeTitles)
                    .HasForeignKey(d => d.MovieMetadataId);
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasIndex(e => e.MovieFileId, "IX_Movies_MovieFileId");

                entity.HasIndex(e => e.MovieMetadataId, "IX_Movies_MovieMetadataId")
                    .IsUnique();

                entity.HasIndex(e => e.Path, "IX_Movies_Path");

                entity.Property(e => e.Added).HasColumnType("DATETIME");

                entity.Property(e => e.LastSearchTime).HasColumnType("DATETIME");

                entity.Property(e => e.Path).IsRequired();

                entity.HasOne(d => d.MovieMetadata)
                    .WithOne(p => p.Movie)
                    .HasForeignKey<Movie>(d => d.MovieMetadataId);
            });

            modelBuilder.Entity<MovieMetadata>(entity =>
            {
                entity.HasIndex(e => e.CleanOriginalTitle, "IX_MovieMetadata_CleanOriginalTitle");

                entity.HasIndex(e => e.CleanTitle, "IX_MovieMetadata_CleanTitle");

                entity.HasIndex(e => e.CollectionTmdbId, "IX_MovieMetadata_CollectionTmdbId");

                entity.HasIndex(e => e.TmdbId, "IX_MovieMetadata_TmdbId")
                    .IsUnique();

                entity.Property(e => e.CleanTitle).IsRequired();

                entity.Property(e => e.DigitalRelease).HasColumnType("DATETIME");

                entity.Property(e => e.Images).IsRequired();

                entity.Property(e => e.InCinemas).HasColumnType("DATETIME");

                entity.Property(e => e.LastInfoSync).HasColumnType("DATETIME");

                entity.Property(e => e.PhysicalRelease).HasColumnType("DATETIME");

                entity.Property(e => e.Recommendations).IsRequired();

                entity.Property(e => e.Title).IsRequired();
            });
        }
    }
}
