
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaBooking.Models;
using CinemaBooking.Data;

public class CinemasController : Controller
{
    private readonly ApplicationDbContext _context;

    public CinemasController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: CINEMAS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Cinemas.ToListAsync());
    }

    // GET: CINEMAS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cinema = await _context.Cinemas
            .FirstOrDefaultAsync(m => m.Id == id);
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
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Imag,Movies")] Cinema cinema)
    {
        if (ModelState.IsValid)
        {
            _context.Add(cinema);
            await _context.SaveChangesAsync();
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

        var cinema = await _context.Cinemas.FindAsync(id);
        if (cinema == null)
        {
            return NotFound();
        }
        return View(cinema);
    }

    // POST: CINEMAS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                _context.Update(cinema);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CinemaExists(cinema.Id))
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

        var cinema = await _context.Cinemas
            .FirstOrDefaultAsync(m => m.Id == id);
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
        var cinema = await _context.Cinemas.FindAsync(id);
        if (cinema != null)
        {
            _context.Cinemas.Remove(cinema);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CinemaExists(int? id)
    {
        return _context.Cinemas.Any(e => e.Id == id);
    }
}
