using DocumentFormat.OpenXml.Spreadsheet;
using Fresh_Farm_Market_214142Z.Model;
using Fresh_Farm_Market_214142Z.Services;
using Fresh_Farm_Market_214142Z.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fresh_Farm_Market_214142Z.Pages
{
    public class ForgetPasswordModel : PageModel
    {

        [BindProperty]
        public ForgetPassword FModel { get; set; }

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UserService _userService;
        private readonly AuditService _auditDb;
        private readonly IEmailService _emailService;
        private UserService userService { get; }
        public ForgetPasswordModel(SignInManager<ApplicationUser> signInManager,
            UserService userService, AuditService auditDb, IEmailService emailService,
            UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._userService = userService;
            this._auditDb = auditDb;
            this._emailService = emailService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var user = await _userManager.FindByEmailAsync(FModel.Email);
            if (user == null)
            {
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = "If the email exists, " +
                    $"an email will be sent to {FModel.Email}";
                return RedirectToPage("/Login");
            }


            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Page("/NewPassword", null, new { Email = user.Email, code = code }, Request.Scheme);
            var message = new Message(
                new string[] { FModel.Email },
                "Test",
            $"Reset your password: {callbackUrl}"
                );
            _emailService.SendEmail(message);
            TempData["FlashMessage.Type"] = "success";
            TempData["FlashMessage.Text"] = "If the email exists, " +
                    $"an email will be sent to {FModel.Email}";
            return RedirectToPage("Login");
        }


    }
}

