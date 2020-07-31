using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeautySalonSystem.UI.Pages
{
    [Authorize]
    public class MyOffers : PageModel
    {
        public void OnGet()
        {
            
        }
    }
}