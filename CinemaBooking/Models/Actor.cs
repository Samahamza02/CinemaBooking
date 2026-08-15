using System.ComponentModel.DataAnnotations;

namespace CinemaBooking.Models
{
    public class Actor
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Imag { get; set; } =string.Empty;

    }
}
