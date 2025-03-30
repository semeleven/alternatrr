using alternatrr.Data.Radarr;
using System.Collections.Generic;

namespace alternatrr.Models.Radarr
{
    public class RadarrMappingsViewModel
    {
        public Movie Movie { get; set; }

        public IList<SceneMapping> SceneMappings { get; set; }
    }
}
