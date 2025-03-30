#nullable disable

using System.Collections.Generic;

namespace alternatrr.Data.Radarr
{
    public partial class MovieMetadata
    {
        public MovieMetadata()
        {
            AlternativeTitles = new HashSet<AlternativeTitle>();
        }

        public long Id { get; set; }
        public long TmdbId { get; set; }
        public string ImdbId { get; set; }
        public string Images { get; set; }
        public string Genres { get; set; }
        public string Title { get; set; }
        public string SortTitle { get; set; }
        public string CleanTitle { get; set; }
        public string OriginalTitle { get; set; }
        public string CleanOriginalTitle { get; set; }
        public long OriginalLanguage { get; set; }
        public long Status { get; set; }
        public byte[] LastInfoSync { get; set; }
        public long Runtime { get; set; }
        public byte[] InCinemas { get; set; }
        public byte[] PhysicalRelease { get; set; }
        public byte[] DigitalRelease { get; set; }
        public long? Year { get; set; }
        public long? SecondaryYear { get; set; }
        public string Ratings { get; set; }
        public string Recommendations { get; set; }
        public string Certification { get; set; }
        public string YouTubeTrailerId { get; set; }
        public string Studio { get; set; }
        public string Overview { get; set; }
        public string Website { get; set; }
        public decimal? Popularity { get; set; }
        public long? CollectionTmdbId { get; set; }
        public string CollectionTitle { get; set; }

        public virtual ICollection<AlternativeTitle> AlternativeTitles { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
