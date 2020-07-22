using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using BeautySalonSystem.UI.Models;
using BeautySalonSystem.UI.Util;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BeautySalonSystem.UI.Services
{
    public interface IAppointmentsService
    {
        void CreateAppointmentRequest(AppointmentCreateInputModel input, string accessToken);

        IEnumerable<AppointmentViewModel> GetConfirmedByCustomerId(string customerId, string accessToken);
    }
    
    public class AppointmentsService : IAppointmentsService
    {
        private readonly HttpClient _client;
        private string _appointmentsBaseUrl;

        public AppointmentsService(IConfiguration configuration)
        {
            Configuration = configuration;
            _client = new HttpClient();
            _appointmentsBaseUrl = Configuration.GetSection("Services:Appointments:Url").Value + "appointments";
        }
        
        public IConfiguration Configuration { get; }
        
        public void CreateAppointmentRequest(AppointmentCreateInputModel input, string accessToken)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = new JsonContent(
                new {
                    input.CustomerId,
                    input.Date,
                    input.OfferId,
                    IsConfirmed = false
                });

            var response = _client.PostAsync(_appointmentsBaseUrl, content).Result;
            Console.WriteLine();
        }

        public IEnumerable<AppointmentViewModel> GetConfirmedByCustomerId(string customerId, string accessToken)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string requestUrl = $"{_appointmentsBaseUrl}?customerId={customerId}";
            var response = _client.GetAsync(requestUrl).Result;
            string responseBody = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<IEnumerable<AppointmentViewModel>>(responseBody);
            return result;
        }
    }
}