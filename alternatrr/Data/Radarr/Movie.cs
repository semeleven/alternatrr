#nullable disable

namespace alternatrr.Data.Radarr
{
    public partial class Movie
    {
        public long Id { get; set; }
        public string Path { get; set; }
        public long Monitored { get; set; }
        public long QualityProfileId { get; set; }
        public byte[] Added { get; set; }
        public string Tags { get; set; }
        public string AddOptions { get; set; }
        public long MovieFileId { get; set; }
        public long MinimumAvailability { get; set; }
        public long MovieMetadataId { get; set; }
        public byte[] LastSearchTime { get; set; }

        public virtual MovieMetadata MovieMetadata { get; set; }
    }
}
