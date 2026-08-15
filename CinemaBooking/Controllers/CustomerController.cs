using CinemaBooking.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBooking.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IMovieRepository _movieRepository;

        public CustomerController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        // Customer Home
        public async Task<IActionResult> Index()
        {
            var movies = await _movieRepository.GetAvailableAsync(6);
            return View(movies);
        }

        // All Movies
        public async Task<IActionResult> Movies()
        {
            var movies = await _movieRepository.GetAllWithDetailsAsync();
            return View(movies);
        }
    }
}
