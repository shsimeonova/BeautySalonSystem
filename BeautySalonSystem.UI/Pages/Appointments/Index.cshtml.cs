using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BeautySalonSystem.UI.Models;
using BeautySalonSystem.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeautySalonSystem.UI.Pages.Appointments
{
    [Authorize]
    public class Index : PageModel
    {
        private IAppointmentsService _appointmentsService;

        public Index(IAppointmentsService appointmentsService)
        {
            _appointmentsService = appointmentsService;
        }
        
        [BindProperty, Required]
        [PageRemote(
            ErrorMessage ="Датата не може да бъде в миналото.",
            AdditionalFields = "__RequestVerificationToken",
            HttpMethod = "post",  
            PageHandler = "CheckIsDateBeforeNow"
        )]
        public DateTime AppointmentRequestDate { get; set; }
        
        [BindProperty]
        public int OfferId { get; set; }
        
        public void OnGet(int id)
        {
            OfferId = id;
        }
        
        public IActionResult OnPost()
        {
            string accessToken = HttpContext.GetTokenAsync("access_token").Result;
            var currentUserId = HttpContext.User.Claims
                .FirstOrDefault(cl => cl.Type.Equals("sub"))
                ?.Value;
            
            _appointmentsService.CreateAppointmentRequest(new AppointmentCreateInputModel
            {
                CustomerId = currentUserId,
                Date = AppointmentRequestDate,
                OfferId = OfferId
            }, 
            accessToken);
            
            return RedirectToPage("/Offers/Index");
        }
        
        public JsonResult OnPostCheckIsDateBeforeNow()
        {
            var isBeforeNow = AppointmentRequestDate > DateTime.Now;
            return new JsonResult(isBeforeNow);
        }
    }
}