using System.ComponentModel.DataAnnotations;

namespace CinemaBooking.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]

        public string Name { get; set; } = string.Empty;
        public List<Movie> Movies { get; set; }


    }
}
