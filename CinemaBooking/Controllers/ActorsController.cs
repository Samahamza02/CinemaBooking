
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaBooking.Models;
using CinemaBooking.Data;

public class ActorsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ActorsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: ACTORS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Actors.ToListAsync());
    }

    // GET: ACTORS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var actor = await _context.Actors
            .FirstOrDefaultAsync(m => m.Id == id);
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
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Imag")] Actor actor)
    {
        if (ModelState.IsValid)
        {
            _context.Add(actor);
            await _context.SaveChangesAsync();
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

        var actor = await _context.Actors.FindAsync(id);
        if (actor == null)
        {
            return NotFound();
        }
        return View(actor);
    }

    // POST: ACTORS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                _context.Update(actor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActorExists(actor.Id))
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

        var actor = await _context.Actors
            .FirstOrDefaultAsync(m => m.Id == id);
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
        var actor = await _context.Actors.FindAsync(id);
        if (actor != null)
        {
            _context.Actors.Remove(actor);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ActorExists(int? id)
    {
        return _context.Actors.Any(e => e.Id == id);
    }
}
