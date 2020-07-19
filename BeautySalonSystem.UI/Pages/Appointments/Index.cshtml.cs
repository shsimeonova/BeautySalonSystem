using System;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeautySalonSystem.UI.Pages.Appointments
{
    public class Index : PageModel
    {
        public void OnGet()
        {
            
        }
        
        public void OnPost(int offerId)
        {
            Console.WriteLine();
        }
    }
}