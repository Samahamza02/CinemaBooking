using CinemaBooking.Models;

namespace CinemaBooking.Repositories
{

    public interface IMovieRepository : IRepository<Movie>
    {
        Task<List<Movie>> GetAllWithDetailsAsync();
        Task<List<Movie>> GetAvailableAsync(int take);
    }
}
