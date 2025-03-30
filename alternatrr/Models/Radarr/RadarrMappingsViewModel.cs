using alternatrr.Data.Radarr;
using System.Collections.Generic;

namespace alternatrr.Models.Radarr
{
    public class RadarrMappingsViewModel
    {
        public Movie Movie { get; set; }
        public MovieMetadata MovieMetadata { get; set; }
        public IList<AlternativeTitle> AlternativeTitles { get; set; }
    }
}
