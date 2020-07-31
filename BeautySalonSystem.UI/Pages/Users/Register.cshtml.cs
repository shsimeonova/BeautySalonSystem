using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeautySalonSystem.UI.Pages
{
    public class Register : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            var authProperies = new AuthenticationProperties();
            authProperies.RedirectUri = "/";
            authProperies.Items.Add("action", "register");
            
            return new ChallengeResult("oidc", authProperies);
        }
    }
}