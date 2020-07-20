using System;
using System.Net.Http;
using System.Net.Http.Headers;
using BeautySalonSystem.UI.Models;
using BeautySalonSystem.UI.Util;
using Microsoft.Extensions.Configuration;

namespace BeautySalonSystem.UI.Services
{
    public interface IAppointmentsService
    {
        void CreateAppointmentRequest(AppointmentCreateInputModel input, string accessToken);
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
    }
}