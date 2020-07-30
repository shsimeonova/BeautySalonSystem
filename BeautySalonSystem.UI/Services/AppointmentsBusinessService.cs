using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BeautySalonSystem.Messages;
using BeautySalonSystem.UI.Models;
using MassTransit;

namespace BeautySalonSystem.UI.Services
{
    public interface IAppointmentsBusinessService
    {
        IEnumerable<AppointmentAdminViewModel> GetAllNonConfirmedAdminView();
        AppointmentAdminViewModel GetByIdAdminView(int id);
        void Confirm(int appointmentRequestId);

    }
    public class AppointmentsBusinessService : IAppointmentsBusinessService
    {
        private IAppointmentsService _appointmentsService;
        private IOffersService _offersService;
        private IUserService _userService;
        private IMapper _mapper;
        private readonly IBus _publisher;

        public AppointmentsBusinessService(
            IAppointmentsService appointmentsService,
            IOffersService offersService,
            IUserService userService,
            IMapper mapper,
            IBus publisher)
        {
            _appointmentsService = appointmentsService;
            _offersService = offersService;
            _userService = userService;
            _mapper = mapper;
            _publisher = publisher;
        }
        
        public IEnumerable<AppointmentAdminViewModel> GetAllNonConfirmedAdminView()
        {
            List<AppointmentViewModel> appointmentRequests = _appointmentsService.GetAllNonConfirmed().ToList();
            HashSet<int> offerIds = appointmentRequests.Select(a => a.offerId).ToHashSet();
            List<AppointmentAdminViewModel> result = new List<AppointmentAdminViewModel>();
            if (appointmentRequests.Count > 0)
            {
                Dictionary<int, OfferViewModel> offers = _offersService.GetManyByIds(offerIds.ToArray(), true);
                appointmentRequests.ForEach(a => a.offer = offers[a.offerId]);
                foreach (var request in appointmentRequests)
                {
                    AppointmentAdminViewModel viewModel = _mapper.Map<AppointmentViewModel, AppointmentAdminViewModel>(request);
                    viewModel.UserInfo = _userService.GetUserPersonalInfo(request.customerId);
                    result.Add(viewModel);
                }
            }
            return result;
        }

        public AppointmentAdminViewModel GetByIdAdminView(int id)
        {
            AppointmentViewModel appointment = _appointmentsService.GetById(id);
            AppointmentAdminViewModel result = new AppointmentAdminViewModel();
            if (appointment != null)
            {
                OfferViewModel offer = _offersService.GetById(appointment.offerId);
                result = _mapper.Map<AppointmentViewModel, AppointmentAdminViewModel>(appointment);
                result.UserInfo = _userService.GetUserPersonalInfo(appointment.customerId);
                result.offer = offer;
            }

            return result;
        }
        
        public void Confirm(int appointmentRequestId)
        {
            bool isConfirmed = _appointmentsService.Confirm(appointmentRequestId);
            
            if (isConfirmed)
            {
                // Task.Run(() =>
                // {
                    var appointment = GetByIdAdminView(appointmentRequestId);

                    _publisher.Publish(new AppointmentConfirmedMessage
                    {
                        CustomerEmail = appointment.UserInfo.Email,
                        AppointmentTime = appointment.Date,
                        OfferName = appointment.offer.Name
                    });
                // });
            }
        }
    }
}