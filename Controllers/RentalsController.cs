using GBMovieRentalSite.Areas.Identity.Data;
using GBMovieRentalSite.Models;
using GBMovieRentalSite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GBMovieRentalSite.Controllers
{
    [Authorize] // Endast inloggade users
    public class RentalsController : Controller
    {
        private readonly ApplicationContext _context;

        public RentalsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Rentals/Rent/5
        // Visar formulär för att välja hyresperiod
        public async Task<IActionResult> Rent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            // Kolla om filmen finns tillgänglig
            if (movie.AvailableCopies <= 0)
            {
                TempData["ErrorMessage"] = "Sorry, this movie is currently out of stock.";
                return RedirectToAction("Details", "Movies", new { id = id });
            }

            var viewModel = new RentalViewModel
            {
                MovieId = movie.Id,
                MovieTitle = movie.Title,
                PricePerDay = movie.PricePerDay,
                AvailableCopies = movie.AvailableCopies,
                RentalDays = 1
            };

            return View(viewModel);
        }

        // POST: Rentals/Confirm
        // Tar emot vald hyresperiod och visar bekräftelse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(RentalViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Rent", model);
            }

            var movie = await _context.Movies.FindAsync(model.MovieId);
            if (movie == null || movie.AvailableCopies <= 0)
            {
                TempData["ErrorMessage"] = "This movie is no longer available.";
                return RedirectToAction("Index", "Movies");
            }

            var confirmModel = new RentalConfirmViewModel
            {
                MovieId = movie.Id,
                MovieTitle = movie.Title,
                MovieGenre = movie.Genre,
                RentalDays = model.RentalDays,
                PricePerDay = movie.PricePerDay,
                TotalPrice = movie.PricePerDay * model.RentalDays,
                RentalDate = DateTime.Now,
                ReturnDate = DateTime.Now.AddDays(model.RentalDays)
            };

            return View(confirmModel);
        }

        // POST: Rentals/Process
        // Skapar faktisk uthyrning i databasen
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Process(int movieId, int rentalDays)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var movie = await _context.Movies.FindAsync(movieId);

            if (movie == null || movie.AvailableCopies <= 0)
            {
                TempData["ErrorMessage"] = "This movie is no longer available.";
                return RedirectToAction("Index", "Movies");
            }

            // Skapa uthyrning
            var rental = new Rental
            {
                UserId = userId,
                MovieId = movieId,
                RentalDate = DateTime.Now,
                ReturnDate = DateTime.Now.AddDays(rentalDays),
                TotalPrice = movie.PricePerDay * rentalDays,
                Status = "Active"
            };

            _context.Rentals.Add(rental);

            // Minska tillgängliga kopior
            movie.AvailableCopies--;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"You have successfully rented '{movie.Title}'!";
            return RedirectToAction(nameof(MyRentals));
        }

        // GET: Rentals/MyRentals
        // Visar användarens alla uthyrningar
        public async Task<IActionResult> MyRentals()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var rentals = await _context.Rentals
                .Include(r => r.Movie)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RentalDate)
                .Select(r => new MyRentalsViewModel
                {
                    RentalId = r.Id,
                    MovieId = r.MovieId,
                    MovieTitle = r.Movie.Title,
                    MovieGenre = r.Movie.Genre,
                    RentalDate = r.RentalDate,
                    ReturnDate = r.ReturnDate,
                    ActualReturnDate = r.ActualReturnDate,
                    TotalPrice = r.TotalPrice,
                    Status = r.Status
                })
                .ToListAsync();

            return View(rentals);
        }

        // POST: Rentals/Return/5
        // Återlämnar en film
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var rental = await _context.Rentals
                .Include(r => r.Movie)
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (rental == null)
            {
                return NotFound();
            }

            if (rental.Status == "Returned")
            {
                TempData["ErrorMessage"] = "This movie has already been returned.";
                return RedirectToAction(nameof(MyRentals));
            }

            // Uppdatera rental
            rental.ActualReturnDate = DateTime.Now;
            rental.Status = "Returned";

            // Öka tillgängliga kopior
            rental.Movie.AvailableCopies++;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"'{rental.Movie.Title}' has been returned successfully!";
            return RedirectToAction(nameof(MyRentals));
        }
    }
}