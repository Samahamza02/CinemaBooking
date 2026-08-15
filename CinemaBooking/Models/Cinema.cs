using System.ComponentModel.DataAnnotations;

namespace CinemaBooking.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Imag { get; set; }

        public List<Movie> Movies { get; set; }
    }
}
