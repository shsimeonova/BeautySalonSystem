using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Http;
using BeautySalonSystem.UI.Models;
using BeautySalonSystem.UI.Util;
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
        
        bool CheckIsAppointmentRequestTimeFree(DateTime appointmentRequestDate, int appointmentRequestDuration);
    }
    
    public class AppointmentsService : MicroserviceHttpService, IAppointmentsService
    {
        public AppointmentsService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) : base(httpClient, httpContextAccessor)
        {}
        
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

        public bool CheckIsAppointmentRequestTimeFree(DateTime appointmentRequestDate, int appointmentRequestDuration)
        {
            MicroserviceResponse response = Execute(
                $"{_client.BaseAddress}/check-time", 
                new
                {
                    AppointmentRequestTime = appointmentRequestDate,
                    AppointmentRequestDuration = appointmentRequestDuration
                }, 
                HttpMethod.Post);
            
            return bool.Parse(response.ReturnData);
        }
    }
}