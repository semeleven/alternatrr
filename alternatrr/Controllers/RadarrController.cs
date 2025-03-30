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
                Movies = await _radarrDbContext.Movies.Include(m => m.MovieMetadata).ToListAsync()
            });
        }

        public async Task<IActionResult> Mappings(long id)
        {
            var movie = await _radarrDbContext.Movies.Include(m => m.MovieMetadata).FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null) return View("Error");

            var movieMetadata = movie.MovieMetadata;
            var alternativeTitles = await _radarrDbContext.AlternativeTitles
                .Where(x => x.MovieMetadataId == movieMetadata.Id)
                .ToListAsync();

            return View(new RadarrMappingsViewModel()
            {
                Movie = movie,
                MovieMetadata = movieMetadata,
                AlternativeTitles = alternativeTitles
            });
        }

        [HttpGet]
        public async Task<IActionResult> AddMapping(long id)
        {
            var movie = await _radarrDbContext.Movies.Include(m => m.MovieMetadata).FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null) return View("Error");

            return View(new RadarrAddMappingInputModel()
            {
                Movie = movie,
                MovieMetadata = movie.MovieMetadata,
                MovieId = movie.Id,
                MovieMetadataId = movie.MovieMetadata.Id
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddMapping(RadarrAddMappingInputModel model)
        {
            var movie = await _radarrDbContext.Movies.Include(m => m.MovieMetadata).FirstOrDefaultAsync(x => x.Id == model.MovieId);
            if (movie == null) return View("Error");

            await _radarrDbContext.AlternativeTitles.AddAsync(new AlternativeTitle()
            {
                MovieMetadataId = movie.MovieMetadata.Id,
                Title = model.SearchTerm,
                CleanTitle = _sceneMappingService.CleanParseTitle(model.SearchTerm),
                SourceType = 0 // 0 seems to be the value for user-added titles based on the sample data
            });
            await _radarrDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Mappings), new { id = model.MovieId });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteMapping(long id)
        {
            var alternativeTitle = await _radarrDbContext.AlternativeTitles.FirstOrDefaultAsync(x => x.Id == id);
            if (alternativeTitle == null) return View("Error");

            var movieMetadata = await _radarrDbContext.MovieMetadata
                .FirstOrDefaultAsync(x => x.Id == alternativeTitle.MovieMetadataId);
            if (movieMetadata == null) return View("Error");

            var movie = await _radarrDbContext.Movies
                .FirstOrDefaultAsync(x => x.MovieMetadataId == movieMetadata.Id);
            if (movie == null) return View("Error");

            return View(new RadarrDeleteMappingViewModel()
            {
                AlternativeTitleId = alternativeTitle.Id,
                Movie = movie,
                MovieMetadata = movieMetadata,
                AlternativeTitle = alternativeTitle
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMapping(RadarrDeleteMappingViewModel model)
        {
            if (!ModelState.IsValid) return View("Error");

            var alternativeTitle = await _radarrDbContext.AlternativeTitles
                .FirstOrDefaultAsync(x => x.Id == model.AlternativeTitleId);
            if (alternativeTitle == null) return View("Error");

            var movieMetadata = await _radarrDbContext.MovieMetadata
                .FirstOrDefaultAsync(x => x.Id == alternativeTitle.MovieMetadataId);
            if (movieMetadata == null) return View("Error");

            var movie = await _radarrDbContext.Movies
                .FirstOrDefaultAsync(x => x.MovieMetadataId == movieMetadata.Id);
            if (movie == null) return View("Error");

            _radarrDbContext.AlternativeTitles.Remove(alternativeTitle);
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
