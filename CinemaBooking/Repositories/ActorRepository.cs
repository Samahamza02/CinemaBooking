using CinemaBooking.Data;
using CinemaBooking.Models;

namespace CinemaBooking.Repositories
{
    public class ActorRepository : Repository<Actor>, IActorRepository
    {
        public ActorRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
