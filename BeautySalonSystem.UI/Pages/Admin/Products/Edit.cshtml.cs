using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using BeautySalonSystem.UI.Models;
using BeautySalonSystem.UI.Services;
using BeautySalonSystem.UI.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BeautySalonSystem.UI.Pages.Admin.Products
{
    public class Edit : PageModel
    {
        private IProductsService _productsService;
        private readonly HttpClient _client;

        public Edit(IConfiguration configuration, IProductsService productsService)
        {
            Configuration = configuration;
            _productsService = productsService;
            _client = new HttpClient();
        }
        
        public IConfiguration Configuration { get; }
        
        [BindProperty]
        public int Id { get; set; }
        
        [BindProperty, Required, MinLength(3)]
        public string Name { get; set; }
        
        [BindProperty, Required]
        [PageRemote(
            ErrorMessage ="Цената на услугата трябва да е минимум 5 лева.",
            AdditionalFields = "__RequestVerificationToken",
            PageName = "./Create",
            HttpMethod ="post",  
            PageHandler ="CheckPrice"
        )]
        public decimal Price { get; set; }
        
        [BindProperty, Required]
        public string Type { get; set; }

        public ProductViewModel ViewModel { get; set; }
        
        [BindProperty]
        public IEnumerable<SelectListItem> Products { get; set; }
        
        public IEnumerable<SelectListItem> TypeOptions { get; set; }

        public void OnGet(int id)
        {
            ViewModel = _productsService.GetById(id);

            var getProductTypesResponse = _productsService.GetProductTypes();
            TypeOptions = getProductTypesResponse.Select(type =>
                new SelectListItem
                {
                    Value = type,
                    Text = type
                }).ToList();
        }

        public IActionResult OnPost()
        {
            _productsService.Edit(new ProductEditViewModel
            {
                Id = Id,
                Name = Name,
                Price =  Price,
                Type = Type
            });
            return RedirectToPage("./Index");
        }
    }
}