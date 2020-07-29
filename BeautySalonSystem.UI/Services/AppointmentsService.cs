using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using BeautySalonSystem.Messages;
using Microsoft.Extensions.Http;
using BeautySalonSystem.UI.Models;
using BeautySalonSystem.UI.Util;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using HttpMethod = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;

namespace BeautySalonSystem.UI.Services
{
    public interface IAppointmentsService
    {
        void CreateAppointmentRequest(AppointmentCreateInputModel input);

        IEnumerable<AppointmentViewModel> GetConfirmedByCustomerId(string customerId);
        
        IEnumerable<AppointmentViewModel> GetAllNonConfirmed();
        
        AppointmentFreeValidationResponseModel CheckIsAppointmentRequestTimeFree(DateTime appointmentRequestDate, int appointmentRequestDuration);

        void Confirm(int appointmentRequstId);
    }
    
    public class AppointmentsService : MicroserviceHttpService, IAppointmentsService
    {
        private readonly IBus _publisher;
        public AppointmentsService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IBus publisher) :
            base(httpClient, httpContextAccessor)
        {
            _publisher = publisher;
        }
        
        public IConfiguration Configuration { get; }
        
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

        public void Confirm(int appointmentRequestId)
        {
            MicroserviceResponse response = Execute(
                $"{_client.BaseAddress}/{appointmentRequestId}/confirm", 
                null, 
                HttpMethod.Post);

            if (response.Code == HttpStatusCode.OK)
            {
                _publisher.Publish(new AppointmentConfirmedMessage
                {
                    AppointmentId = appointmentRequestId
                });
            }
        }
    }
}