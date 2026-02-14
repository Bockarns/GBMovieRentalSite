using GBMovieRentalSite.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace GBMovieRentalSite.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(50)]
        public string? Genre { get; set; }

        [StringLength(100)]
        public string? Director { get; set; }

        public int? ReleaseYear { get; set; }

        [Required]
        public decimal PricePerDay { get; set; }

        [Required]
        public int TotalCopies { get; set; }

        [Required]
        public int AvailableCopies { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties (dessa är för relationerna)
        public ApplicationUser? User { get; set; }
        public string? UserId { get; set; }
    }
}
