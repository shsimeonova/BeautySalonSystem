using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeautySalonSystem.UI.Pages
{
    public class Register : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            return new ChallengeResult("oidc");
        }
    }
}