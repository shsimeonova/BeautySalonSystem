using System;
using System.Collections.Generic;
using System.Net.Http;
using BeautySalonSystem.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using HttpMethod = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;

namespace BeautySalonSystem.UI.Services
{
    public interface IOffersService
    {
        IEnumerable<OfferViewModel> GetAll(bool activeOnly);
        IEnumerable<OfferViewModel> GetAllActive();
        OfferViewModel GetById(int id);
        Dictionary<int, OfferViewModel> GetManyByIds(int[] ids, bool activeOnly);
        void Create(OfferCreateInputModel input);
        void Delete(int id);
        void Activate(int id);
        int GetDuration(int id);
    }
    
    public class OffersService : MicroserviceHttpService, IOffersService
    {
        public OffersService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) : base(httpClient, httpContextAccessor)
        {}
        
        public IConfiguration Configuration { get; }

        public IEnumerable<OfferViewModel> GetAll(bool activeOnly)
        {
            MicroserviceResponse response = Execute(
                $"{_client.BaseAddress}?activeOnly={activeOnly}",
                null,
                HttpMethod.Get);
            
            var allOffers = JsonConvert.DeserializeObject<IEnumerable<OfferViewModel>>(response.ReturnData);

            return allOffers;
        }

        public IEnumerable<OfferViewModel> GetAllActive()
        {
            var offers = GetAll(true);
            Console.WriteLine();
            return offers;
        }

        public OfferViewModel GetById(int id)
        {
            MicroserviceResponse response = Execute(
                $"{_client.BaseAddress}?id={id}",
                null,
                HttpMethod.Get);
            
            var offer = JsonConvert.DeserializeObject<OfferViewModel>(response.ReturnData);

            return offer;
        }
        
        public Dictionary<int, OfferViewModel> GetManyByIds(int[] ids, bool activeOnly)
        {
            string requestUrl = $"{_client.BaseAddress}/all/?activeOnly={activeOnly}";
            
            foreach (int id in ids)
            {
                requestUrl += $"&ids={id}";
            }
            
            MicroserviceResponse response = Execute(
                requestUrl,
                null,
                HttpMethod.Get);
            
            var offers = JsonConvert.DeserializeObject<IEnumerable<OfferViewModel>>(response.ReturnData);
            
            Dictionary<int, OfferViewModel> result = new Dictionary<int, OfferViewModel>();
            foreach (var offerViewModel in offers)
            {
                result.Add(offerViewModel.Id, offerViewModel);
            }

            return result;
        }

        public void Create(OfferCreateInputModel input)
        {
            MicroserviceResponse response = Execute(
                _client.BaseAddress.ToString(),
                new {
                    input.Name,
                    input.TotalPrice,
                    Products = input.ProductIds,
                    input.Discount,
                    input.ExpiryDate,
                    input.ImageUrl
                },
                HttpMethod.Post);
            
            Console.WriteLine();
        }

        public void Delete(int id)
        {
            MicroserviceResponse response = Execute(
                $"{_client.BaseAddress}/{id}",
                null,
                HttpMethod.Delete);
            
            Console.WriteLine();
        }

        public void Activate(int id)
        {
            MicroserviceResponse response = Execute(
                $"{_client.BaseAddress}/activate/{id}",
                null,
                HttpMethod.Get);
            
            Console.WriteLine();
        }

        public int GetDuration(int id)
        {
            MicroserviceResponse response = Execute(
                $"{_client.BaseAddress}/{id}/duration",
                null,
                HttpMethod.Get);
            
            return int.Parse(response.ReturnData);
        }
    }
}