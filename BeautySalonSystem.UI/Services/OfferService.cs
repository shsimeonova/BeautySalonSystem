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
    public interface IOfferService
    {
        void Create(CreateOfferInputModel input, string accessToken);
    }
    
    public class OfferService : IOfferService
    {
        private readonly HttpClient _client;
        private string _offersBaseUrl;
        
        public OfferService(IConfiguration configuration)
        {
            Configuration = configuration;
            _offersBaseUrl = Configuration.GetSection("Services:Products:Url").Value + "productoffers";
            _client = new HttpClient();
        }
        
        public IConfiguration Configuration { get; }
        
        public async void Create(CreateOfferInputModel input, string accessToken)
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
            
            var response = await _client.PostAsync(_offersBaseUrl, content);
            string responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine();
        }
    }
}