
using Microsoft.AspNetCore.Mvc;
using CinemaBooking.Models;
using CinemaBooking.Repositories;

public class MoviesController : Controller
{
    private readonly IMovieRepository _movieRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICinemaRepository _cinemaRepository;
    private readonly IActorRepository _actorRepository;

    public MoviesController(
        IMovieRepository movieRepository,
        ICategoryRepository categoryRepository,
        ICinemaRepository cinemaRepository,
        IActorRepository actorRepository)
    {
        _movieRepository = movieRepository;
        _categoryRepository = categoryRepository;
        _cinemaRepository = cinemaRepository;
        _actorRepository = actorRepository;
    }

    // GET: MOVIES
    public async Task<IActionResult> Index()
    {
        return View(await _movieRepository.GetAllAsync());
    }

    // GET: MOVIES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _movieRepository.GetByIdAsync(id.Value);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // GET: MOVIES/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _categoryRepository.GetAllAsync();
        ViewBag.Cinemas = await _cinemaRepository.GetAllAsync();
        ViewBag.Actors = await _actorRepository.GetAllAsync();
        return View();
    }

    // POST: MOVIES/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Des,Price,Status,DateTime,MainImg,SubImages,Actors,CategoryId,Category,CinemaId,Cinema")] Movie movie)
    {
        if (ModelState.IsValid)
        {
            await _movieRepository.AddAsync(movie);
            await _movieRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(movie);
    }

    // GET: MOVIES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _movieRepository.GetByIdAsync(id.Value);
        if (movie == null)
        {
            return NotFound();
        }
        return View(movie);
    }

    // POST: MOVIES/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Des,Price,Status,DateTime,MainImg,SubImages,Actors,CategoryId,Category,CinemaId,Cinema")] Movie movie)
    {
        if (id != movie.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _movieRepository.Update(movie);
                await _movieRepository.SaveChangesAsync();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
            {
                if (!await _movieRepository.ExistsAsync(movie.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(movie);
    }

    // GET: MOVIES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _movieRepository.GetByIdAsync(id.Value);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // POST: MOVIES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var movie = id.HasValue ? await _movieRepository.GetByIdAsync(id.Value) : null;
        if (movie != null)
        {
            _movieRepository.Remove(movie);
        }

        await _movieRepository.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
