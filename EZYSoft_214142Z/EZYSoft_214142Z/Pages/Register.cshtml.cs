using AspNetCore.ReCaptcha;
using EZYSoft_214142Z.ViewModels;
using Fresh_Farm_Market_214142Z.Model;
using Fresh_Farm_Market_214142Z.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Identity.Client;
using System.Text;
using System.Text.RegularExpressions;

namespace Fresh_Farm_Market_214142Z.Pages
{
    //Initialize the build-in ASP.NET Identity
    //[ValidateReCaptcha]

    [ValidateAntiForgeryToken]
    public class RegisterModel : PageModel
    {
        private UserManager<ApplicationUser> userManager { get; }

        private SignInManager<ApplicationUser> signInManager { get; }

        private readonly RoleManager<IdentityRole> roleManager;

        private readonly IEmailService _emailService;

        private IWebHostEnvironment _environment;

        [BindProperty]
        public Register RModel { get; set; }

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IEmailService emailService,
            IWebHostEnvironment environment
            )

        {
            this.userManager = userManager;
            this._emailService = emailService;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            _environment = environment;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return RedirectToPage("/email/ConfirmEmail");

            var result = await userManager.ConfirmEmailAsync(user, token);
            return RedirectToPage(result.Succeeded ? "/email/ConfirmEmail" : "/email/ErrorEmail");
        }


        //Save data into the database

        public async Task<IActionResult> OnPostAsync()
        {

            if (ModelState.IsValid)

            {
                var uploads = "/uploads/user.jpg";
                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");

                var AboutMeBytes = Encoding.UTF8.GetBytes(RModel.AboutMe);
                var AboutMeEncoded = Convert.ToBase64String(AboutMeBytes);


                if (!Regex.IsMatch(RModel.Password, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*[@$!%*#?&])[\S].{12,}$"))
                {
                    if (!Regex.IsMatch(RModel.Password, @"(?=.*[0-9])"))
                    {
                        ModelState.AddModelError("Password", "Passwords must contain at least one digit");
                    }
                    if (!Regex.IsMatch(RModel.Password, @"(?=.*[a-z])"))
                    {
                        ModelState.AddModelError("Password", "Passwords must contain at least one lowercase letter");
                    }
                    if (!Regex.IsMatch(RModel.Password, @"(?=.*[A-Z])"))
                    {
                        ModelState.AddModelError("Password", "Passwords must contain at least one uppercase letter");
                    }
                    if (!Regex.IsMatch(RModel.Password, @".{12,}"))
                    {
                        ModelState.AddModelError("Password", "Passwords must contain at least 12 characters");
                    }
                    if (!Regex.IsMatch(RModel.Password, @"(?=.*[@$!%*#?&])"))
                    {
                        ModelState.AddModelError("Password", "Passwords must contain at least one special character");
                    }
                    if (!Regex.IsMatch(RModel.Password, @"[\S]"))
                    {
                        ModelState.AddModelError("Password", "Passwords cannot contain any spaces");
                    }

                    Console.WriteLine("Got problem");
                    return Page();
                }

                var user = new ApplicationUser()

                    {
                        UserName = RModel.Email,
                        FullName = RModel.FullName,
                        CreditCard = protector.Protect(RModel.CreditCard),
                        Gender = RModel.Gender,
                        MobileNo = RModel.MobileNo,
                        DeliveryAddress = RModel.DeliveryAddress,
                        Email = RModel.Email,
                        ImageURL = uploads,
                        AboutMe = AboutMeEncoded

                    };


                IdentityRole role = await roleManager.FindByIdAsync("Admin");

                if (role == null)
                {
                    IdentityResult result1 = await roleManager.CreateAsync(new IdentityRole("Admin"));

                    if (!result1.Succeeded)

                    {
                        ModelState.AddModelError("", "Cretae role admin failed");
                    }

                }

                role = await roleManager.FindByIdAsync("HR");

                if (role == null)
                {
                    IdentityResult result2 = await roleManager.CreateAsync(new IdentityRole("HR"));

                    if (!result2.Succeeded)

                    {
                        ModelState.AddModelError("", "Cretae role HR failed");
                    }

                }


                var result = await userManager.CreateAsync(user, RModel.Password);



                if (result.Succeeded)
                    {

                    var result3 = await userManager.AddToRoleAsync(user, "Admin");
                    
                    result3 = await userManager.AddToRoleAsync(user, "HR");


                    await signInManager.SignInAsync(user, false);
                        return RedirectToPage("Index");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }
                return Page();
            }
        }
    }

