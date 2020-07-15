using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using BeautySalonSystem.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BeautySalonSystem.UI.Services
{
    public interface IProductsService
    {
        IEnumerable<ProductViewModel> GetAll(string accessToken);
        void Create(ProductCreateInputModel input, HttpRequest request);
        void Delete(int id, HttpRequest request);
        IEnumerable<string> GetProductTypes(HttpRequest request);
    }
    
    public class ProductsService : IProductsService
    {
        private readonly HttpClient _client;
        private string _productsBaseUrl;
        
        public ProductsService(IConfiguration configuration)
        {
            Configuration = configuration;
            _productsBaseUrl = Configuration.GetSection("Services:Products:Url").Value + "products";
            _client = new HttpClient();
        }
        
        public IConfiguration Configuration { get; }

        private void AttachTokenHeader(HttpRequest request)
        {
            var token = request.Headers["Authorization"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        
        public IEnumerable<ProductViewModel> GetAll(string accessToken)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = _client.GetAsync(this._productsBaseUrl).Result;
            string responseBody = response.Content.ReadAsStringAsync().Result;
            var allProducts = JsonConvert.DeserializeObject<IEnumerable<ProductViewModel>>(responseBody);

            return allProducts;
        }
        
        public void Create(ProductCreateInputModel input, HttpRequest request)
        {
            var token = request.Headers["Authorization"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = JsonConvert.SerializeObject(new
            {
                Name = input.Name,
                Price = input.Price,
                Type = input.Type
            });
            var response = _client.PostAsync(_productsBaseUrl, new StringContent(content,  Encoding.UTF8, "application/json")).Result;
        } 
        
        public void Delete(int id, HttpRequest request)
        {
            AttachTokenHeader(request);
            var response = _client.DeleteAsync(this._productsBaseUrl + $"?id={id}").Result;
        }

        public IEnumerable<string> GetProductTypes(HttpRequest request)
        {
            AttachTokenHeader(request);
            var response = _client.GetAsync(_productsBaseUrl + "/types").Result;
            var responseList = JsonConvert.DeserializeObject<IEnumerable<string>>(response.Content.ReadAsStringAsync().Result);

            return responseList;
        }
    }
}