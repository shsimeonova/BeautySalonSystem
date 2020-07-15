using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeautySalonSystem.UI.Pages.Admin
{
    [Authorize(Policy = "Admin")]
    public class Index : PageModel
    {
        public void OnGet()
        {
        }
    }
}