using Fresh_Farm_Market_214142Z.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fresh_Farm_Market_214142Z.Pages
{

        [Authorize]
        public class Index1Model : PageModel
        {

            private UserManager<ApplicationUser> userManager { get; }

            public Index1Model(UserManager<ApplicationUser> userManager)

            {
                this.userManager = userManager;

            }

            [BindProperty]
            public ApplicationUser AppData1 { get; set; }


            public async Task<IActionResult> OnGet()
            {

                AppData1 = await userManager.GetUserAsync(User);

                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");

                var protector = dataProtectionProvider.CreateProtector("MySecretKey");

                //AppData1.CreditCard = protector.Unprotect(AppData1.CreditCard);

                return Page();
            }

        }
    }


