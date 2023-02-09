using Fresh_Farm_Market_214142Z.Model;
using Fresh_Farm_Market_214142Z.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fresh_Farm_Market_214142Z.Pages.email
{
    public class confirmModel : PageModel
    {

        private UserManager<ApplicationUser> userManager { get; }
        private readonly UserService _userService;

        public confirmModel(UserManager<ApplicationUser> userManager, UserService userService)
        {
            this.userManager = userManager;
            this._userService = userService;
        }

        public async Task<IActionResult> OnGet(string email, string code)
        {


            Console.WriteLine("Email: " + email);
            Console.WriteLine("Token: " + code);
            var user = _userService.GetUserByEmail(email);
            if (user == null)
            {
                Console.WriteLine("user not found brother");
                return Redirect("/email/error");
            }

            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return Page();
            }
            else
            {
                Console.WriteLine("result could not be confirmed");
                return Redirect("/email/error");
            }

        }
    }
}

