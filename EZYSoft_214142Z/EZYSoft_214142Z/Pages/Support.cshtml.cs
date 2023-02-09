using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fresh_Farm_Market_214142Z.Pages
{
    [Authorize(Roles = "Admin")]
    public class SupportModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
