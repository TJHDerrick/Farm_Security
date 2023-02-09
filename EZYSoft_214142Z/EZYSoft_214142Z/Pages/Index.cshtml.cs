using Fresh_Farm_Market_214142Z.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EZYSoft_214142Z.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {

        private UserManager<ApplicationUser> userManager { get; }

        public IndexModel(UserManager<ApplicationUser> userManager)
           
        {
            this.userManager = userManager;
                
        }

       [BindProperty]
        public ApplicationUser AppData { get; set;}



        public async Task<IActionResult> OnGet()
        {

            AppData = await userManager.GetUserAsync(User);

            var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");

            var protector = dataProtectionProvider.CreateProtector("MySecretKey");

            AppData.CreditCard = protector.Unprotect(AppData.CreditCard);

            return Page();
        }

       


    }
}