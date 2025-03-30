using alternatrr.Data.Radarr;
using alternatrr.Models;
using alternatrr.Models.Radarr;
using alternatrr.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace alternatrr.Controllers
{
    [Authorize]
    public class RadarrController : Controller
    {
        private readonly RadarrDbContext _radarrDbContext;
        private readonly SceneMappingService _sceneMappingService;

        public RadarrController(RadarrDbContext radarrDbContext, SceneMappingService sceneMappingService)
        {
            _radarrDbContext = radarrDbContext;
            _sceneMappingService = sceneMappingService;
        }

        public async Task<IActionResult> Index()
        {
            return View(new RadarrIndexViewModel
            {
                Movies = await _radarrDbContext.Movies.ToListAsync()
            });
        }

        public async Task<IActionResult> Mappings(long id)
        {
            var movie = await _radarrDbContext.Movies.FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null) return View("Error");

            var mappings = await _radarrDbContext.SceneMappings.Where(x => x.TmdbId == movie.TmdbId).ToListAsync();

            return View(new RadarrMappingsViewModel()
            {
                Movie = movie,
                SceneMappings = mappings
            });
        }

        [HttpGet]
        public async Task<IActionResult> AddMapping(long id)
        {
            var movie = await _radarrDbContext.Movies.FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null) return View("Error");

            return View(new RadarrAddMappingInputModel()
            {
                Movie = movie,
                MovieId = movie.Id,
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddMapping(RadarrAddMappingInputModel model)
        {
            var movie = await _radarrDbContext.Movies.FirstOrDefaultAsync(x => x.Id == model.MovieId);
            if (movie == null) return View("Error");

            await _radarrDbContext.SceneMappings.AddAsync(new Data.Radarr.SceneMapping()
            {
                TmdbId = movie.TmdbId,
                ParseTerm = _sceneMappingService.CleanParseTitle(model.SearchTerm),
                SearchTerm = _sceneMappingService.CleanSearchTitle(model.SearchTerm),
                Title = model.SearchTerm,
                Type = "alternatrr"
            });
            await _radarrDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Mappings), new { id = model.MovieId });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteMapping(long id)
        {
            var mapping = await _radarrDbContext.SceneMappings.FirstOrDefaultAsync(x => x.Id == id);
            if (mapping == null) return View("Error");

            var movie = await _radarrDbContext.Movies.FirstOrDefaultAsync(x => x.TmdbId == mapping.TmdbId);

            return View(new RadarrDeleteMappingViewModel()
            {
                MappingId = mapping.Id,
                Movie = movie,
                SceneMapping = mapping
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMapping(RadarrDeleteMappingViewModel model)
        {
            if (!ModelState.IsValid) return View("Error");

            var mapping = await _radarrDbContext.SceneMappings.FirstOrDefaultAsync(x => x.Id == model.MappingId);
            if (mapping == null) return View("Error");

            var movie = await _radarrDbContext.Movies.FirstOrDefaultAsync(x => x.TmdbId == mapping.TmdbId);
            if (movie == null) return View("Error");

            _radarrDbContext.SceneMappings.Remove(mapping);
            await _radarrDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Mappings), new { id = movie.Id });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
