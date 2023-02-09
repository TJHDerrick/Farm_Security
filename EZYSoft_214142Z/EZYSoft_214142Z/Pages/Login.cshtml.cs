using AspNetCore.ReCaptcha;
using Fresh_Farm_Market_214142Z.Model;
using Fresh_Farm_Market_214142Z.Services;
using Fresh_Farm_Market_214142Z.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NETCore.MailKit.Core;
using System.Collections;
using System.Security.Claims;


namespace Fresh_Farm_Market_214142Z.Pages
{
    //[ValidateReCaptcha]
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Login LModel { get; set; }

        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly AuditService AuditDb;

        public LoginModel(SignInManager<ApplicationUser> signInManager, AuditService AuditDb)
        {
            this.signInManager = signInManager;
            this.AuditDb = AuditDb;
        }

        [BindProperty]

        public IEnumerable<AuthenticationScheme> ExternalLoginProviders { get; set; }

        public async Task OnGetAsync()
        {
            this.ExternalLoginProviders = await signInManager.GetExternalAuthenticationSchemesAsync();
        }

        public IActionResult OnPostLoginExternally (string provider)

        {

            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, null);

            properties.RedirectUri = Url.Action("ExternalLoginCallBack", "Account");

            return Challenge(properties, provider);

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {

                var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, 
                    LModel.Password,
                    LModel.RememberMe, /*false,*/ 
                    lockoutOnFailure: true);

                //if (identityResult.Succeeded)
                //{

                    //Create the security context
                    var claims = new List<Claim>
                    {

                        new Claim(ClaimTypes.Name, "c@c.com"),
                        new Claim(ClaimTypes.Email, "c@c.com")
                    };

                    var i = new ClaimsIdentity(claims, "MyCookieAuth");

                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(i);

                    await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

                    Audit audit = new Audit();
                    audit.AuditID = Guid.NewGuid().ToString();
                    audit.UserID = LModel.Email;
                    audit.AuditAction = "Login";
                    audit.TimeStamp = DateTime.Now.ToString("F");
                    AuditDb.AddAudit(audit);

                    return RedirectToPage("Index");
                //}

                ModelState.AddModelError("", "Username or Password incorrect");
                Console.WriteLine("Invalid username or password");

            }
            return Page();
        }
    }
}
