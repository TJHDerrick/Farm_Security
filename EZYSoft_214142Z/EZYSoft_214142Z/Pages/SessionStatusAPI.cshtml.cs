using Fresh_Farm_Market_214142Z.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fresh_Farm_Market_214142Z.Pages
{
    public class SessionStatusAPIModel : PageModel
    {
        private UserManager<ApplicationUser> userManager { get; }
        public SessionStatusAPIModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                return Content("loggedin");
            }
            else
            {
                return Content("loggedout");
            }
        }
    }
}
