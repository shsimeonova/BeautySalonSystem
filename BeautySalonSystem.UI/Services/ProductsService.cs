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
    public interface IProductsService
    {
        IEnumerable<ProductViewModel> GetAll();
        ProductViewModel GetById(int id);
        void Create(ProductCreateInputModel input);
        void Edit(ProductEditViewModel input);
        void Delete(int id);
        IEnumerable<string> GetProductTypes();
    }
    
    public class ProductsService : MicroserviceHttpService, IProductsService
    {
        public ProductsService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) : base(httpClient, httpContextAccessor)
        {}
        
        public IConfiguration Configuration { get; }
        
        public IEnumerable<ProductViewModel> GetAll()
        {
            MicroserviceResponse response = Execute(
                _client.BaseAddress.ToString(),
                null,
                HttpMethod.Get);
            
            var allProducts = JsonConvert.DeserializeObject<IEnumerable<ProductViewModel>>(response.ReturnData);

            return allProducts;
        }
        
        public ProductViewModel GetById(int id)
        {
            MicroserviceResponse response = Execute(
                $"{_client.BaseAddress}/{id}",
                null,
                HttpMethod.Get);
            
            ProductViewModel result = JsonConvert.DeserializeObject<ProductViewModel>(response.ReturnData);

            return result;
        }
        
        public void Create(ProductCreateInputModel input)
        {
            MicroserviceResponse response = Execute(
                _client.BaseAddress.ToString(),
                input,
                HttpMethod.Post);

            Console.WriteLine();
        }

        public void Edit(ProductEditViewModel input)
        {
            MicroserviceResponse response = Execute(
               $"{_client.BaseAddress}/{input.Id}",
                input,
                HttpMethod.Put);
            
            Console.WriteLine();
        }

        public void Delete(int id)
        {
            MicroserviceResponse response = Execute(
                $"{_client.BaseAddress}?id={id}",
                null,
                HttpMethod.Put);
        }

        public IEnumerable<string> GetProductTypes()
        {
            MicroserviceResponse response = Execute(
                $"{_client.BaseAddress}/types",
                null,
                HttpMethod.Get);
            
            var responseList = JsonConvert.DeserializeObject<IEnumerable<string>>(response.ReturnData);
            return responseList;
        }
    }
}