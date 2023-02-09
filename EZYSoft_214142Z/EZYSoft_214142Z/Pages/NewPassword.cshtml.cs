using DocumentFormat.OpenXml.EMMA;
using Fresh_Farm_Market_214142Z.Model;
using Fresh_Farm_Market_214142Z.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;

namespace Fresh_Farm_Market_214142Z.Pages
{
    public class NewPasswordModel : PageModel
    {
        private UserManager<ApplicationUser> userManager { get; }


        public NewPasswordModel(UserManager<ApplicationUser> userManager
)
        {

            this.userManager = userManager;
        }

        [BindProperty]
        public NewPassword NModel { get; set; } = new();
        public async Task<IActionResult> OnGet(string email, string code)
        {
            Console.WriteLine(email);
            Console.WriteLine(code);
            //check if email is null
            if (email == null)
            {
                Console.WriteLine("Email does not exist");
                return RedirectToPage("/email/error");
            }

            //check if code is null
            if (code == null)
            {
                Console.WriteLine("Code does not exist");
                return RedirectToPage("/email/error");
            }
            Console.WriteLine($"Before email: {email}");
            Console.WriteLine($"Before code: {code}");
            NModel.Email = email;
            NModel.Code = code;
            //check if user is null
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                Console.WriteLine("user does not exist brother");
                return RedirectToPage("/email/error");
            }

            var result = await userManager.VerifyUserTokenAsync(user, userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", code);
            //var result = await userManager.ResetPasswordAsync(user, code);
            if (result == true)
            {
                //Console.WriteLine("succeed");
                //user.EmailConfirmed = true;
                //await userManager.UpdateAsync(user);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("Email confirmed successfully! You can now reset your password.");

                return Page();
            }
            else
            {
                Console.WriteLine("not succeed");
                return RedirectToPage("/email/error");
            }


        }

        public async Task<IActionResult> OnPost()
        {


            Console.WriteLine($"EmailR: {NModel.Email}");
            Console.WriteLine($"codeR: {NModel.Code}");
           

            if (!Regex.IsMatch(NModel.Password, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*[@$!%*#?&])[\S].{12,}$"))
            {
                if (!Regex.IsMatch(NModel.Password, @"(?=.*[0-9])"))
                {
                    ModelState.AddModelError("Password", "Passwords must contain at least one digit");
                }
                if (!Regex.IsMatch(NModel.Password, @"(?=.*[a-z])"))
                {
                    ModelState.AddModelError("Password", "Passwords must contain at least one lowercase letter");
                }
                if (!Regex.IsMatch(NModel.Password, @"(?=.*[A-Z])"))
                {
                    ModelState.AddModelError("Password", "Passwords must contain at least one uppercase letter");
                }
                if (!Regex.IsMatch(NModel.Password, @".{12,}"))
                {
                    ModelState.AddModelError("Password", "Passwords must contain at least 12 characters");
                }
                if (!Regex.IsMatch(NModel.Password, @"(?=.*[@$!%*#?&])"))
                {
                    ModelState.AddModelError("Password", "Passwords must contain at least one special character");
                }
                if (!Regex.IsMatch(NModel.Password, @"[\S]"))
                {
                    ModelState.AddModelError("Password", "Passwords cannot contain any spaces");
                }
                Console.WriteLine("Got problem");
                return Page();
            }


            var user = await userManager.FindByEmailAsync(NModel.Email);

            var result = await userManager.ResetPasswordAsync(user, NModel.Code, NModel.Password);

            //var result = await userManager.ChangePasswordAsync(user, user.PasswordHash, Cmodel.Password);
            if (result.Succeeded)
            {
                await userManager.UpdateAsync(user);
                TempData["FlashMessage.Type"] = "success";
                TempData["FlashMessage.Text"] = string.Format("Password changes successfully, please try login in now");
                return RedirectToPage("Login");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);

            }
            return Page();






        }
    }
    } 
