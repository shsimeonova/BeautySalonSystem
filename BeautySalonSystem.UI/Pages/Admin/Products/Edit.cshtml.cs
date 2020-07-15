using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using BeautySalonSystem.UI.Models;
using BeautySalonSystem.UI.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BeautySalonSystem.UI.Pages.Admin.Products
{
    public class Edit : PageModel
    {
        private string _productsBaseUrl;
        private readonly HttpClient _client;

        public Edit(IConfiguration configuration)
        {
            Configuration = configuration;
            _productsBaseUrl = Configuration.GetSection("Services:Products:Url").Value + "products";
            _client = new HttpClient();
        }
        
        public IConfiguration Configuration { get; }
        
        [BindProperty]
        public string Id { get; set; }
        
        [BindProperty, Required]
        public string Name { get; set; }
        [BindProperty, Required]
        public decimal Price { get; set; }
        [BindProperty, Required]
        public string Type { get; set; }

        public ProductViewModel ViewModel { get; set; }
        
        public IEnumerable<SelectListItem> TypeOptions { get; set; }

        public void OnGet(int id)
        {
            var token = HttpContext.Request.Headers["Authorization"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var getProductResponse = _client.GetAsync(_productsBaseUrl + $"/{id}").Result;
            ViewModel = JsonConvert.DeserializeObject<ProductViewModel>(getProductResponse.Content.ReadAsStringAsync().Result);
            
            var getProductTypesResponse = _client.GetAsync(_productsBaseUrl + "/types").Result;
            var getProductTypesResponseContent = JsonConvert.DeserializeObject<IEnumerable<string>>(getProductTypesResponse.Content.ReadAsStringAsync().Result);
            TypeOptions = getProductTypesResponseContent.Select(type =>
                new SelectListItem
                {
                    Value = type,
                    Text = type
                }).ToList();
            Console.WriteLine();
        }

        public IActionResult OnPost()
        {
            var token = HttpContext.Request.Headers["Authorization"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var content = new JsonContent(new {
                Name = this.Name,
                Price = this.Price,
                Type = this.Type
            });
            
            var getProductResponse = _client.PutAsync(_productsBaseUrl + $"/{Id}", content).Result;
            return RedirectToPage("./Index");
        }
    }
}