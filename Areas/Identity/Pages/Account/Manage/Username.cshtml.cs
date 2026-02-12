using GBMovieRentalSite.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace GBMovieRentalSite.Areas.Identity.Pages.Account.Manage
{
    public class UsernameModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsernameModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public string CurrentUserName { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "New Username")]
            [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            public string NewUserName { get; set; }
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            CurrentUserName = user.UserName;

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                CurrentUserName = user.UserName;
                return Page();
            }

            var currentUserName = user.UserName;

            // Kolla om det är samma som nuvarande
            if (Input.NewUserName == currentUserName)
            {
                StatusMessage = "Your username is unchanged.";
                return RedirectToPage();
            }

            // Kolla om det nya användarnamnet redan finns
            var existingUser = await _userManager.FindByNameAsync(Input.NewUserName);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Username is already taken.");
                CurrentUserName = currentUserName;
                return Page();
            }

            // Uppdatera UserName
            var setUserNameResult = await _userManager.SetUserNameAsync(user, Input.NewUserName);
            if (!setUserNameResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to set username.";
                return RedirectToPage();
            }

            // Logga in användaren igen för att uppdatera claims
            await _signInManager.RefreshSignInAsync(user);

            StatusMessage = "Your username has been changed.";
            return RedirectToPage();
        }
    }
}
