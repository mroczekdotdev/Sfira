using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MroczekDotDev.Sfira.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Areas.Account.Pages
{
    public partial class ProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public ProfileModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public ProfileInputModel Input { get; set; }

        public class ProfileInputModel
        {
            [Required]
            [RegularExpression(@"^[ -~]+$", ErrorMessage = "Name can contain only basic characters.")]
            [StringLength(36, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
            public string Name { get; set; }

            [StringLength(240, ErrorMessage = "{0} can't be longer than {1} characters.")]
            public string Description { get; set; }

            [StringLength(36, ErrorMessage = "{0} can't be longer than {1} characters.")]
            public string Location { get; set; }

            [RegularExpression(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$", ErrorMessage = "URL must be valid.")]
            public string Website { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            var userName = await userManager.GetUserNameAsync(user);
            var name = user.Name;
            var description = user.Description;
            var location = user.Location;
            var website = user.Website;

            Username = userName;

            Input = new ProfileInputModel
            {
                Name = name,
                Description = description,
                Location = location,
                Website = website
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            if (Input.Name != user.Name)
            {
                user.Name = Input.Name;
            }

            if (Input.Description != user.Description)
            {
                user.Description = Input.Description;
            }

            if (Input.Location != user.Location)
            {
                user.Location = Input.Location;
            }

            if (Input.Website != user.Website)
            {
                user.Website = Input.Website;
            }

            await userManager.UpdateAsync(user);

            await signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
