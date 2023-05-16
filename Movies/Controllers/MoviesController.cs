using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Data;
using Movies.ViewModel;
using NToastNotify;

namespace Movies.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toast;



        public MoviesController(ApplicationDbContext context , IToastNotification toast)
        {
            _context = context;
            _toast = toast;
        }
        public async Task<IActionResult> Index()
        {
            var Movie = await _context.Movies.Include( m=> m.Genra).ToListAsync();
            return View(Movie);
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Create()
        {
            var movie = new MovieViewModel
            {
                Genras = await _context.Genras.ToListAsync(),
            };
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieViewModel model)
        {
            var files = Request.Form.Files;
            if (!files.Any())
            {
                model.Genras = await _context.Genras.ToListAsync();
                ModelState.AddModelError("Poster", "Please Select Poster...");
                return View(model);
            }
            var poster = files.FirstOrDefault();
            var allowExtenstions = new List<string> { ".jpg", ".png" };
            if (!allowExtenstions.Contains(Path.GetExtension(poster.FileName).ToLower()))
            {
                model.Genras = await _context.Genras.ToListAsync();
                ModelState.AddModelError("Poster", "Only .jpg and .png Extenstions...");
                return View(model);
            }

            using var datastream = new MemoryStream();
            await poster.CopyToAsync(datastream);
            var movie = new Movie
            {
                Title = model.Title,
                StoryLine = model.StoryLine,
                Rate = model.Rate,
                GenraId = model.GenraId,
                Poster = datastream.ToArray()
            };

            _context.Movies.Add(movie);
            _context.SaveChanges();
            _toast.AddSuccessToastMessage("Movie Added Successfully");
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();
            var movie = await _context.Movies.FindAsync(id);
            if(movie == null)
                return NotFound();
            var viewmodel = new MovieViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                StoryLine = movie.StoryLine,
                Rate = movie.Rate,
                GenraId = movie.GenraId,
                Poster = movie.Poster,
                Genras = await _context.Genras.ToListAsync(),
            };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieViewModel model)
        {
            var movie = await _context.Movies.FindAsync(model.Id);
            if (movie == null)
                return NotFound();
            var files = Request.Form.Files;
            if (files.Any())
            {
                var poster = files.FirstOrDefault();
                using var datastream = new MemoryStream();
                await poster.CopyToAsync(datastream);
                model.Poster = datastream.ToArray();

                movie.Poster = model.Poster;
            }

            movie.Title = model.Title;
            movie.StoryLine = model.StoryLine;
            movie.Rate = model.Rate;
            movie.GenraId = model.GenraId;

            _context.SaveChanges();
            _toast.AddSuccessToastMessage("Movie Edited Successfully");

            return RedirectToAction("Index");

        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var movie = await _context.Movies.FindAsync(id);
            if(movie == null)
                return NotFound();

            _context.Movies.Remove(movie);
             _context.SaveChanges();
            _toast.AddErrorToastMessage("Movie Removed Successfully");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
                return BadRequest();
            var movie = await _context.Movies.Include(m => m.Genra).SingleOrDefaultAsync(m => m.Id == id);
            if(movie == null)
                return NotFound();

            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string term)
        {
            var book = await _context.Movies.Include(m => m.Genra).Where(m => m.Title.Contains(term)).SingleOrDefaultAsync();
            return View(book);
        }
    }
}
