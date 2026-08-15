
using Microsoft.AspNetCore.Mvc;
using CinemaBooking.Models;
using CinemaBooking.Repositories;

public class ActorsController : Controller
{
 
    private readonly IActorRepository _actorRepository;

    public ActorsController(IActorRepository actorRepository)
    {
        _actorRepository = actorRepository;
    }

    // GET: ACTORS
    public async Task<IActionResult> Index()
    {
        return View(await _actorRepository.GetAllAsync());
    }

    // GET: ACTORS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var actor = await _actorRepository.GetByIdAsync(id.Value);
        if (actor == null)
        {
            return NotFound();
        }

        return View(actor);
    }

    // GET: ACTORS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ACTORS/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Imag")] Actor actor)
    {
        if (ModelState.IsValid)
        {
            await _actorRepository.AddAsync(actor);
            await _actorRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(actor);
    }

    // GET: ACTORS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var actor = await _actorRepository.GetByIdAsync(id.Value);
        if (actor == null)
        {
            return NotFound();
        }
        return View(actor);
    }

    // POST: ACTORS/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Imag")] Actor actor)
    {
        if (id != actor.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _actorRepository.Update(actor);
                await _actorRepository.SaveChangesAsync();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
            {
                if (!await _actorRepository.ExistsAsync(actor.Id))
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
        return View(actor);
    }

    // GET: ACTORS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var actor = await _actorRepository.GetByIdAsync(id.Value);
        if (actor == null)
        {
            return NotFound();
        }

        return View(actor);
    }

    // POST: ACTORS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var actor = id.HasValue ? await _actorRepository.GetByIdAsync(id.Value) : null;
        if (actor != null)
        {
            _actorRepository.Remove(actor);
        }

        await _actorRepository.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
