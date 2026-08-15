using CinemaBooking.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaBooking.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }


        // Customer Home
        public IActionResult Index()
        {
            var movies = _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .Where(m => m.Status == "Available")
                .Take(6)
                .ToList();

            return View(movies);
        }


        // All Movies
        public IActionResult Movies()
        {
            var movies = _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .ToList();

            return View(movies);
        }
    }
}