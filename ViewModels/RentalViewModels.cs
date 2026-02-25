using System.ComponentModel.DataAnnotations;

namespace GBMovieRentalSite.ViewModels
{
    // ViewModel för att välja hyresperiod
    public class RentalViewModel
    {
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public decimal PricePerDay { get; set; }
        public int AvailableCopies { get; set; }

        [Required]
        [Range(1, 7, ErrorMessage = "Rental period must be between 1 and 7 days")]
        [Display(Name = "Rental Period (days)")]
        public int RentalDays { get; set; } = 1;

        public decimal TotalPrice => PricePerDay * RentalDays;
    }

    // ViewModel för bekräftelse-sidan
    public class RentalConfirmViewModel
    {
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public string MovieGenre { get; set; }
        public int RentalDays { get; set; }
        public decimal PricePerDay { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }

    // ViewModel för att visa användarens uthyrningar
    public class MyRentalsViewModel
    {
        public int RentalId { get; set; }
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public string MovieGenre { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }

        // Hjälpproperties
        public bool IsOverdue => ReturnDate < DateTime.Now && ActualReturnDate == null;
        public bool CanReview => ActualReturnDate != null && Status == "Returned";
    }
}