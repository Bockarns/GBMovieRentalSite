using GBMovieRentalSite.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace GBMovieRentalSite.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public int RentalId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(2000)]
        public string? Comment { get; set; }

        [Required]
        public DateTime ReviewDate { get; set; } = DateTime.Now;

        // Navigation properties
        public ApplicationUser User { get; set; }
        public Movie Movie { get; set; }
        public Rental Rental { get; set; }
    }
}
