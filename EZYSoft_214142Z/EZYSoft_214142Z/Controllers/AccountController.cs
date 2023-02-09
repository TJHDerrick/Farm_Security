using Fresh_Farm_Market_214142Z.Model;
using Fresh_Farm_Market_214142Z.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fresh_Farm_Market_214142Z.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly AuditService _auditDb;
            
        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, AuditService auditDb)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this._auditDb = auditDb;    
        }

        public async Task<IActionResult> ExternalLoginCallBack()
        {
            var loginInfo = await signInManager.GetExternalLoginInfoAsync();

            var emailClaim = loginInfo.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

            var userClaim = loginInfo.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            if (emailClaim != null && userClaim != null)

            {

                var user = new ApplicationUser { Email = emailClaim.Value, UserName = userClaim.Value };

                await signInManager.SignInAsync(user, false);

                var result = await userManager.FindByEmailAsync(emailClaim.Value);
                
                if (result != null)
                {
                    Audit audit = new Audit();
                    audit.AuditID = Guid.NewGuid().ToString();
                    audit.UserID = result.Email;
                    audit.AuditAction = "Login";
                    audit.TimeStamp = DateTime.Now.ToString("F");
                    _auditDb.AddAudit(audit);
                    Console.WriteLine($"Audit iuwdbuidfbweuihfewuifhewui = \n {audit} ");
                    await userManager.UpdateSecurityStampAsync(result);
                    await signInManager.SignInAsync(result, false);
                    return RedirectToPage("/Index");
                }

            }

            return RedirectToPage("/Index1");

        }
    }
}

