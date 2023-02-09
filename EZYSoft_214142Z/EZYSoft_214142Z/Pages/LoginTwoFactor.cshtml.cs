using Fresh_Farm_Market_214142Z.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NETCore.MailKit.Core;

namespace Fresh_Farm_Market_214142Z.Pages
{
    public class LoginTwoFactorModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailService emailService;

        public LoginTwoFactorModel(UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            this.userManager = userManager;
            this.emailService = emailService;   
        }



        public async Task OnGetAsync(string email, string rememberMe)
        {
            var user = await userManager.FindByEmailAsync(email);

            var securityCode = await userManager.GenerateTwoFactorTokenAsync(user, "Email");

            await emailService.SendAsync("derricktanjh@gmai.com",
                email,
                "My Web App's OTP",
                $"Please use this code as the OTP: {securityCode}",
                securityCode);
        }
    }
}
