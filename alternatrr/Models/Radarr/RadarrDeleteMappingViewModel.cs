using alternatrr.Data.Radarr;
using System.ComponentModel.DataAnnotations;

namespace alternatrr.Models.Radarr
{
    public class RadarrDeleteMappingViewModel
    {
        [Required]
        public long AlternativeTitleId { get; set; }

        public Movie Movie { get; set; }
        public MovieMetadata MovieMetadata { get; set; }
        public AlternativeTitle AlternativeTitle { get; set; }
    }
}
