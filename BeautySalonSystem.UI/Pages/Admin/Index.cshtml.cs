using System;
using System.Collections;
using System.Collections.Generic;
using BeautySalonSystem.UI.Models;
using BeautySalonSystem.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeautySalonSystem.UI.Pages.Admin
{
    [Authorize(Policy = "Admin")]
    public class Index : PageModel
    {
        public IAppointmentsService _appointmentsService;

        public Index(IAppointmentsService appointmentsService)
        {
            _appointmentsService = appointmentsService;
        }

        public IEnumerable<AppointmentViewModel> Appointments { get; set; }
        public void OnGet()
        {
            // string accessToken = HttpContext.GetTokenAsync("access_token").Result;
            // Console.WriteLine();
        }
    }
}