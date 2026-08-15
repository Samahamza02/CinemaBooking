using CinemaBooking.Data;
using CinemaBooking.Models;

namespace CinemaBooking.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
