using CinemaBooking.Data;
using CinemaBooking.Models;

namespace CinemaBooking.Repositories
{
    public class CinemaRepository : Repository<Cinema>, ICinemaRepository
    {
        public CinemaRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
