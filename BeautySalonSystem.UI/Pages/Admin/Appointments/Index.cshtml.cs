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
        private IAppointmentsService _appointmentsService;
        private IOffersService _offersService;
        private IUserService _userService;
        private IMapper _mapper;
        
        public Index(IAppointmentsService appointmentsService, IOffersService offersService, IUserService userService, IMapper mapper)
        {
            _appointmentsService = appointmentsService;
            _offersService = offersService;
            _userService = userService;
            _mapper = mapper;
        }

        public IEnumerable<AppointmentAdminViewModel> AppointmentRequests;
        
        public IActionResult OnGet()
        {
            List<AppointmentViewModel> appointmentRequests = _appointmentsService.GetAllNonConfirmed().ToList();
            HashSet<int> offerIds = appointmentRequests.Select(a => a.offerId).ToHashSet();
            if (appointmentRequests.Count > 0)
            {
                Dictionary<int, OfferViewModel> offers = _offersService.GetManyByIds(offerIds.ToArray(), true);
                appointmentRequests.ForEach(a => a.offer = offers[a.offerId]);
                List<AppointmentAdminViewModel> result = new List<AppointmentAdminViewModel>();
                foreach (var request in appointmentRequests)
                {
                    AppointmentAdminViewModel viewModel = _mapper.Map<AppointmentViewModel, AppointmentAdminViewModel>(request);
                    viewModel.UserInfo = _userService.GetUserPersonalInfo(request.customerId);
                    result.Add(viewModel);
                }

                AppointmentRequests = result;
            }

            return Page();
        }

        public void OnPostConfirmAppointmentRequest(int id)
        {
            _appointmentsService.Confirm(id);
            
            OnGet();
        }
    }
}