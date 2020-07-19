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
    public interface IOffersService
    {
        IEnumerable<ViewOfferOutputModel> GetAll();
        void Create(CreateOfferInputModel input, string accessToken);
    }
    
    public class OffersService : IOffersService
    {
        private readonly HttpClient _client;
        private string _offersBaseUrl;
        
        public OffersService(IConfiguration configuration)
        {
            Configuration = configuration;
            _offersBaseUrl = Configuration.GetSection("Services:Products:Url").Value + "offers";
            _client = new HttpClient();
        }
        
        public IConfiguration Configuration { get; }

        public IEnumerable<ViewOfferOutputModel> GetAll()
        {
            var response = _client.GetAsync(_offersBaseUrl).Result;
            string responseBody = response.Content.ReadAsStringAsync().Result;
            var allOffers = JsonConvert.DeserializeObject<IEnumerable<ViewOfferOutputModel>>(responseBody);

            return allOffers;
        }

        public void Create(CreateOfferInputModel input, string accessToken)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = new JsonContent(
            new {
                Name = input.Name,
                TotalPrice = input.TotalPrice,
                Products = input.ProductIds,
                Discount = input.Discount,
                ExpiryDate = input.ExpiryDate
            });
            
            var response = _client.PostAsync(_offersBaseUrl, content).Result;
            string responseBody = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine();
        }
    }
}