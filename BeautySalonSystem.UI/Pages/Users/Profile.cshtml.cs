using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BeautySalonSystem.UI.Models;
using BeautySalonSystem.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeautySalonSystem.UI.Pages.Users
{
    [Authorize]
    public class Profile : PageModel
    {
        private IOffersService _offersService;
        private IAppointmentsService _appointmentsService;

        public Profile(IOffersService offersService, IAppointmentsService appointmentsService)
        {
            _offersService = offersService;
            _appointmentsService = appointmentsService;
        }
        [BindProperty]
        [Required, EmailAddress]
        public string Email { get; set; }
        
        [BindProperty]
        [Required]
        public string UserName { get; set; }
        
        [BindProperty]
        [Required]
        public string FullName { get; set; }
        
        public List<AppointmentViewModel> Appointments { get; set; }
        
        public void OnGet()
        {
            var userClaims = HttpContext.User.Claims;
            UserName = userClaims.FirstOrDefault(c => c.Type.Equals("preferred_username")).Value;
            FullName = userClaims.FirstOrDefault(c => c.Type.Equals("name")).Value;
            Email = userClaims.FirstOrDefault(c => c.Type.Equals("email")).Value;
            
            var currentUserId = HttpContext.User.Claims
                .FirstOrDefault(cl => cl.Type.Equals("sub"))
                ?.Value;
            
            Appointments = _appointmentsService.GetConfirmedByCustomerId(currentUserId).ToList();

            if (Appointments.Count != 0)
            {
                HashSet<int> offerIds = Appointments.Select(a => a.offerId).ToHashSet();
            
                Dictionary<int, OfferViewModel> offers = _offersService.GetManyByIds(offerIds.ToArray(), true);
                Appointments.ForEach(a => a.offer = offers[a.offerId]);
            }
            
            Console.WriteLine();
        }
    }
}