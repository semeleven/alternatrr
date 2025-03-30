#nullable disable

namespace alternatrr.Data.Radarr
{
    public partial class AlternativeTitle
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string CleanTitle { get; set; }
        public long SourceType { get; set; }
        public long MovieMetadataId { get; set; }

        public virtual MovieMetadata MovieMetadata { get; set; }
    }
}
