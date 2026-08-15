
using Microsoft.AspNetCore.Mvc;
using CinemaBooking.Models;
using CinemaBooking.Repositories;

public class CinemasController : Controller
{
    private readonly ICinemaRepository _cinemaRepository;

    public CinemasController(ICinemaRepository cinemaRepository)
    {
        _cinemaRepository = cinemaRepository;
    }

    // GET: CINEMAS
    public async Task<IActionResult> Index()
    {
        return View(await _cinemaRepository.GetAllAsync());
    }

    // GET: CINEMAS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cinema = await _cinemaRepository.GetByIdAsync(id.Value);
        if (cinema == null)
        {
            return NotFound();
        }

        return View(cinema);
    }

    // GET: CINEMAS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CINEMAS/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Imag,Movies")] Cinema cinema)
    {
        if (ModelState.IsValid)
        {
            await _cinemaRepository.AddAsync(cinema);
            await _cinemaRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(cinema);
    }

    // GET: CINEMAS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cinema = await _cinemaRepository.GetByIdAsync(id.Value);
        if (cinema == null)
        {
            return NotFound();
        }
        return View(cinema);
    }

    // POST: CINEMAS/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Imag,Movies")] Cinema cinema)
    {
        if (id != cinema.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _cinemaRepository.Update(cinema);
                await _cinemaRepository.SaveChangesAsync();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
            {
                if (!await _cinemaRepository.ExistsAsync(cinema.Id))
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
        return View(cinema);
    }

    // GET: CINEMAS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cinema = await _cinemaRepository.GetByIdAsync(id.Value);
        if (cinema == null)
        {
            return NotFound();
        }

        return View(cinema);
    }

    // POST: CINEMAS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var cinema = id.HasValue ? await _cinemaRepository.GetByIdAsync(id.Value) : null;
        if (cinema != null)
        {
            _cinemaRepository.Remove(cinema);
        }

        await _cinemaRepository.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
