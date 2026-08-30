using Microsoft.AspNetCore.Identity;

namespace CinemaBooking.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Adderss { get; set; }
    }
}
