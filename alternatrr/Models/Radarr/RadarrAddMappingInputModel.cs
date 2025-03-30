using alternatrr.Data.Radarr;

namespace alternatrr.Models.Radarr
{
    public class RadarrAddMappingInputModel
    {
        public Movie Movie { get; set; }

        public long MovieId { get; set; }

        public string SearchTerm { get; set; }
    }
}
