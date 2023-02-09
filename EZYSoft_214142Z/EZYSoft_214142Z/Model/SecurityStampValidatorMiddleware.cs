using Microsoft.AspNetCore.Identity;

namespace Fresh_Farm_Market_214142Z.Model
{
    public class SecurityStampValidatorMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityStampValidatorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user != null)
            {
                var stamp = await userManager.GetSecurityStampAsync(user);
                if (stamp != user.SecurityStamp)
                {
                    Console.WriteLine("You have been logged out!");
                    await signInManager.SignOutAsync();
                    context.Response.Redirect("/Pages/MultipleSessionInvalid");
                    return;

                }
                Console.WriteLine("You have been hehehehe out!");
            }

            await _next.Invoke(context);
        }
    }
}
