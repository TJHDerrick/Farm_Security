using Fresh_Farm_Market_214142Z.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fresh_Farm_Market_214142Z.Controllers
{
    public class EmailController : ControllerBase
    {
        private UserManager<ApplicationUser> userManager;
        public EmailController(UserManager<ApplicationUser> userManager)
        {
            userManager = userManager;
        }

        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return RedirectToPage("/email/ConfirmEmail");

            var result = await userManager.ConfirmEmailAsync(user, token);
            return RedirectToPage(result.Succeeded ? "/email/ConfirmEmail" : "/email/ErrorEmail");
        }
    }
}
