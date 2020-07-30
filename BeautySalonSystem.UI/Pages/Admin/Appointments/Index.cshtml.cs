using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BeautySalonSystem.UI.Models;
using BeautySalonSystem.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeautySalonSystem.UI.Pages.Admin.Appointments
{
    public class Index : PageModel
    {
        private IAppointmentsBusinessService _appointmentsBusinessServiceService;
        private IMapper _mapper;

        public Index(IAppointmentsBusinessService appointmentsBusinessServiceService)
        {
            _appointmentsBusinessServiceService = appointmentsBusinessServiceService;
        }

        public IEnumerable<AppointmentAdminViewModel> AppointmentRequests;
        
        public IActionResult OnGet()
        {
            AppointmentRequests = _appointmentsBusinessServiceService.GetAllNonConfirmedAdminView();
            return Page();
        }

        public void OnPostConfirmAppointmentRequest(int id)
        {
            _appointmentsBusinessServiceService.Confirm(id);
            OnGet();
        }
    }
}