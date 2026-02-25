using GBMovieRentalSite.Models;
using System.ComponentModel.DataAnnotations;

namespace GBMovieRentalSite.ViewModels
{
    // ViewModel för att skapa review
    public class CreateReviewViewModel
    {
        public int RentalId { get; set; }
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars")]
        [Display(Name = "Rating")]
        public int Rating { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Comment must be between 10 and 2000 characters")]
        [Display(Name = "Your Review")]
        public string Comment { get; set; }
    }

    // ViewModel för att visa review
    public class ReviewDisplayViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public string TimeAgo => CalculateTimeAgo();

        private string CalculateTimeAgo()
        {
            var timeSpan = DateTime.Now - ReviewDate;

            if (timeSpan.TotalDays > 365)
                return $"{(int)(timeSpan.TotalDays / 365)} year(s) ago";
            if (timeSpan.TotalDays > 30)
                return $"{(int)(timeSpan.TotalDays / 30)} month(s) ago";
            if (timeSpan.TotalDays > 1)
                return $"{(int)timeSpan.TotalDays} day(s) ago";
            if (timeSpan.TotalHours > 1)
                return $"{(int)timeSpan.TotalHours} hour(s) ago";
            if (timeSpan.TotalMinutes > 1)
                return $"{(int)timeSpan.TotalMinutes} minute(s) ago";

            return "Just now";
        }
    }

    // ViewModel för filmsidan med reviews
    public class MovieWithReviewsViewModel
    {
        public Movie Movie { get; set; }
        public List<ReviewDisplayViewModel> Reviews { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
    }
}