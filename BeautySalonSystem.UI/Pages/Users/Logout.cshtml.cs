using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeautySalonSystem.UI.Pages.Users
{
    public class Logout : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            return new SignOutResult(new[] { "oidc", "Cookies" });
        }
    }
}