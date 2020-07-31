using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using AutoMapper;
using BeautySalonSystem.UI.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using HttpMethod = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;

namespace BeautySalonSystem.UI.Services
{
    public interface IAppointmentsService
    {
        void CreateAppointmentRequest(AppointmentCreateInputModel input);

        IEnumerable<AppointmentViewModel> GetConfirmedByCustomerId(string customerId);
        
        IEnumerable<AppointmentViewModel> GetAllNonConfirmed();
        
        AppointmentViewModel GetById(int id);

        bool Confirm(int appointmentRequestId);

        AppointmentFreeValidationResponseModel CheckIsAppointmentRequestTimeFree(DateTime appointmentRequestDate,
            int appointmentRequestDuration);
    }
    
    public class AppointmentsService : MicroserviceHttpService, IAppointmentsService
    {
        private IMapper _mapper;
        
        public AppointmentsService(
            IMapper mapper,
            HttpClient httpClient, 
            IHttpContextAccessor httpContextAccessor) 
            : base(httpClient, httpContextAccessor)
        {
            _mapper = mapper;
        }
        
        public void CreateAppointmentRequest(AppointmentCreateInputModel input)
        {
            MicroserviceResponse response = Execute(
                _client.BaseAddress.ToString(), 
                new {
                    input.CustomerId,
                    input.Date,
                    input.OfferId,
                    IsConfirmed = false
                }, 
                HttpMethod.Post);
        }

        public IEnumerable<AppointmentViewModel> GetConfirmedByCustomerId(string customerId)
        {
            MicroserviceResponse response = Execute(
                $"{_client.BaseAddress}/customers/{customerId}/confirmed", 
                null, 
                HttpMethod.Get);
            
            var result = JsonConvert.DeserializeObject<IEnumerable<AppointmentViewModel>>(response.ReturnData);
            
            return result;
        }

        public IEnumerable<AppointmentViewModel> GetAllNonConfirmed()
        {
            MicroserviceResponse response = Execute(
                $"{_client.BaseAddress}/non-confirmed", 
                null, 
                HttpMethod.Get);
            
            var result = JsonConvert.DeserializeObject<IEnumerable<AppointmentViewModel>>(response.ReturnData);
            
            return result;
        }

        public AppointmentFreeValidationResponseModel CheckIsAppointmentRequestTimeFree(DateTime appointmentRequestDate, int appointmentRequestDuration)
        {
            MicroserviceResponse response = Execute(
                $"{_client.BaseAddress}/check-time", 
                new
                {
                    AppointmentRequestTime = appointmentRequestDate,
                    AppointmentRequestDuration = appointmentRequestDuration
                }, 
                HttpMethod.Post);
            
            var result = JsonConvert.DeserializeObject<AppointmentFreeValidationResponseModel>(response.ReturnData);
            return result;
        }

        public AppointmentViewModel GetById(int appointmentId)
        {
            MicroserviceResponse response = Execute(
                $"{_client.BaseAddress}/{appointmentId}", 
                null, 
                HttpMethod.Get);
            
            var result = JsonConvert.DeserializeObject<AppointmentViewModel>(response.ReturnData);
            return result;
        }

        public bool Confirm(int appointmentRequestId)
        {
            MicroserviceResponse response = Execute(
                $"{_client.BaseAddress}/{appointmentRequestId}/confirm", 
                null, 
                HttpMethod.Post);

            return response.Code == HttpStatusCode.OK;
        }
    }
}