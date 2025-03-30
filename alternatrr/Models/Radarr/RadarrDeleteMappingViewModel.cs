using alternatrr.Data.Radarr;
using System.ComponentModel.DataAnnotations;

namespace alternatrr.Models.Radarr
{
    public class RadarrDeleteMappingViewModel
    {
        [Required]
        public long MappingId { get; set; }

        public Movie Movie { get; set; }

        public Data.Radarr.SceneMapping SceneMapping { get; set; }
    }
}
