using GBMovieRentalSite.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace GBMovieRentalSite.Models
{
    public class Rental
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public DateTime RentalDate { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; }

        public DateTime? ActualReturnDate { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Active"; // Active, Returned, Overdue

        // Navigation properties
        public ApplicationUser User { get; set; }
        public Movie Movie { get; set; }
    }
}
