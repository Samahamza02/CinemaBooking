using Microsoft.EntityFrameworkCore;
using CinemaBooking.Data;
using CinemaBooking.Models;

namespace CinemaBooking.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Movie>> GetAllWithDetailsAsync()
        {
            return await _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .ToListAsync();
        }

        public async Task<List<Movie>> GetAvailableAsync(int take)
        {
            return await _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .Where(m => m.Status == "Available")
                .Take(take)
                .ToListAsync();
        }
    }
}
