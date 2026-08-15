using System.ComponentModel.DataAnnotations;

namespace CinemaBooking.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Des { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; }

        public DateTime DateTime { get; set; }

        public string MainImg { get; set; }

        public List<string> SubImages { get; set; } = new List<string>();

        // Many-to-many with Actors
        public List<Actor> Actors { get; set; } = new List<Actor>();

        // Foreign Key  Category
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        // Foreign Key  Cinema
        public int CinemaId { get; set; }

        public Cinema Cinema { get; set; }
    }
}
