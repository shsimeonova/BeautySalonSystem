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
        IEnumerable<OfferViewModel> GetAll(bool activeOnly);
        IEnumerable<OfferViewModel> GetAllActive();
        OfferViewModel GetById(int id, string accessToken);
        Dictionary<int, OfferViewModel> GetManyByIds(int[] ids, bool activeOnly);
        void Create(OfferCreateInputModel input, string accessToken);
        void Delete(int id, string accessToken);
        void Activate(int id, string accessToken);
        
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

        public IEnumerable<OfferViewModel> GetAll(bool activeOnly)
        {
            var response = _client.GetAsync($"{_offersBaseUrl}?activeOnly={activeOnly}").Result;
            string responseBody = response.Content.ReadAsStringAsync().Result;
            var allOffers = JsonConvert.DeserializeObject<IEnumerable<OfferViewModel>>(responseBody);

            return allOffers;
        }

        public IEnumerable<OfferViewModel> GetAllActive()
        {
            var offers = GetAll(true);
            Console.WriteLine();
            return offers;
        }

        public OfferViewModel GetById(int id, string accessToken)
        {
            var requestUrl = $"{_offersBaseUrl}?id={id}";
            var response = _client.GetAsync(requestUrl).Result;
            string responseBody = response.Content.ReadAsStringAsync().Result;
            var offer = JsonConvert.DeserializeObject<OfferViewModel>(responseBody);

            return offer;
        }
        
        public Dictionary<int, OfferViewModel> GetManyByIds(int[] ids, bool activeOnly)
        {
            var requestUrl = $"{_offersBaseUrl}/all/?activeOnly={activeOnly}";
            foreach (int id in ids)
            {
                requestUrl += $"&ids={id}";
            }
            
            var response = _client.GetAsync(requestUrl).Result;
            string responseBody = response.Content.ReadAsStringAsync().Result;
            var offers = JsonConvert.DeserializeObject<IEnumerable<OfferViewModel>>(responseBody);
            
            Dictionary<int, OfferViewModel> result = new Dictionary<int, OfferViewModel>();
            foreach (var offerViewModel in offers)
            {
                result.Add(offerViewModel.Id, offerViewModel);
            }

            return result;
        }

        public void Create(OfferCreateInputModel input, string accessToken)
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

        public void Delete(int id, string accessToken)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var requestUrl = $"{_offersBaseUrl}/{id}";
            var response = _client.DeleteAsync(requestUrl).Result;

            Console.WriteLine();
        }

        public void Activate(int id, string accessToken)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string requestUrl = $"{_offersBaseUrl}/activate/{id}";
            var response = _client.GetAsync(requestUrl).Result;
            string responseBody = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine();
        }
    }
}