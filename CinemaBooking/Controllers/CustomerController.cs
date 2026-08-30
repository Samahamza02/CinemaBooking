<<<<<<< HEAD
﻿using CinemaBooking.Repositories;
using Microsoft.AspNetCore.Mvc;
=======
﻿using CinemaBooking.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
>>>>>>> dbd02d4aa542cd42cd235d0b351c0af497d96ec0

namespace CinemaBooking.Controllers
{
    public class CustomerController : Controller
    {
<<<<<<< HEAD
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
=======
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
>>>>>>> dbd02d4aa542cd42cd235d0b351c0af497d96ec0
