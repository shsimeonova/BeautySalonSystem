using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BeautySalonSystem.UI.Models;
using BeautySalonSystem.UI.Services;
using BeautySalonSystem.UI.Util;
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
        private IOffersService _offersService;

        public Index(IAppointmentsService appointmentsService, IOffersService offersService)
        {
            _appointmentsService = appointmentsService;
            _offersService = offersService;
        }
        
        [BindProperty, Required]
        // [PageRemote(
        //     ErrorMessage = "Датата не може да бъде в миналото",
        //     AdditionalFields = "__RequestVerificationToken",
        //     HttpMethod = "post",  
        //     PageHandler = "IsDateBeforeNow"
        // )]
        public DateTime AppointmentRequestDate { get; set; }
        
        [BindProperty]
        public int OfferId { get; set; }
        
        [BindProperty]
        public int Duration { get; set; }
        
        public PageMessage Message { get; set; }
        
        public void OnGet(int id)
        {
            OfferId = id;
            string accessToken = HttpContext.GetTokenAsync("access_token").Result;
            Duration = _offersService.GetDuration(id, accessToken);
            Console.WriteLine();
        }
        
        public IActionResult OnPost()
        {
            if (!IsDateBeforeNow())
            {
                Message = new PageMessage
                {
                    Type = PageMessageType.Danger,
                    Text = "Датата не може да бъде в миналото."
                };
                return Page();
            }
            
            if (!IsAppointmentTimeFree())
            {
                Message = new PageMessage
                {
                    Type = PageMessageType.Danger,
                    Text = "Тази дата не е свободна."
                };
                return Page();
            }
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

        private bool IsDateBeforeNow()
        {
            var isBeforeNow = AppointmentRequestDate > DateTime.Now;
            return isBeforeNow;
        }
        
        public bool IsAppointmentTimeFree()
        {
            string accessToken = HttpContext.GetTokenAsync("access_token").Result;
            bool result =
                _appointmentsService.CheckIsAppointmentRequestTimeFree(accessToken, AppointmentRequestDate, Duration);
            return result;
        }
    }
}