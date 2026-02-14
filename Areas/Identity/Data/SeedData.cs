
using GBMovieRentalSite.Models;
using Microsoft.AspNetCore.Identity;

namespace GBMovieRentalSite.Areas.Identity.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Definiera roller
            string[] roleNames = { "Admin", "User" };

            // Skapa roller om de inte finns
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Skapa en admin-användare om den inte finns
            var adminEmail = "admin@gbmovierental.se";
            var adminUserName = "admin";
            var adminPassword = "Admin123!"; // Byt till ett säkert lösenord!

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var createAdmin = await userManager.CreateAsync(newAdmin, adminPassword);
                if (createAdmin.Succeeded)
                {
                    // Lägg till admin-användaren i Admin-rollen
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
        public static async Task SeedMovies(ApplicationContext context)
        {
            // Kolla om det redan finns filmer
            if (context.Movies.Any())
            {
                return; // Filmer finns redan, hoppa över
            }

            var movies = new List<Movie>
    {
        new Movie
        {
            Title = "The Shawshank Redemption",
            Description = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
            Genre = "Drama",
            Director = "Frank Darabont",
            ReleaseYear = 1994,
            PricePerDay = 29.99m,
            TotalCopies = 5,
            AvailableCopies = 5,
            CreatedDate = DateTime.Now
        },
        new Movie
        {
            Title = "The Godfather",
            Description = "The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.",
            Genre = "Crime",
            Director = "Francis Ford Coppola",
            ReleaseYear = 1972,
            PricePerDay = 39.99m,
            TotalCopies = 3,
            AvailableCopies = 3,
            CreatedDate = DateTime.Now
        },
        new Movie
        {
            Title = "The Dark Knight",
            Description = "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests.",
            Genre = "Action",
            Director = "Christopher Nolan",
            ReleaseYear = 2008,
            PricePerDay = 34.99m,
            TotalCopies = 4,
            AvailableCopies = 4,
            CreatedDate = DateTime.Now
        },
        new Movie
        {
            Title = "Pulp Fiction",
            Description = "The lives of two mob hitmen, a boxer, a gangster and his wife intertwine in four tales of violence and redemption.",
            Genre = "Crime",
            Director = "Quentin Tarantino",
            ReleaseYear = 1994,
            PricePerDay = 29.99m,
            TotalCopies = 3,
            AvailableCopies = 3,
            CreatedDate = DateTime.Now
        },
        new Movie
        {
            Title = "Inception",
            Description = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea.",
            Genre = "Sci-Fi",
            Director = "Christopher Nolan",
            ReleaseYear = 2010,
            PricePerDay = 34.99m,
            TotalCopies = 4,
            AvailableCopies = 4,
            CreatedDate = DateTime.Now
        }
    };

            context.Movies.AddRange(movies);
            await context.SaveChangesAsync();
        }
    }
}
