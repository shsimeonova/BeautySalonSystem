
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Authentication;
using BeautySalonSystem.UI.Models;
using BeautySalonSystem.UI.Services;
using BeautySalonSystem.UI.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace BeautySalonSystem.UI.Pages.Admin.Products
{
    [Authorize(Policy = "Admin")]
    public class Create : PageModel
    {
        private IProductsService _productsService;
        
        public Create(IProductsService productsService)
        {
            _productsService = productsService;
        }
        
        public IConfiguration Configuration { get; }
        
        [BindProperty, Required, MinLength(3)]
        public string Name { get; set; }
        
        [BindProperty, Required]
        [PageRemote(
            ErrorMessage ="Цената на услугата трябва да е минимум 5 лева.",
            AdditionalFields = "__RequestVerificationToken",
            HttpMethod ="post",  
            PageHandler ="CheckPrice"
        )]
        public decimal Price { get; set; }
        
        [BindProperty, Required]
        public string Type { get; set; }
        
        public IEnumerable<SelectListItem> TypeOptions { get; set; }
        
        public void OnGet()
        {
            string accessToken = HttpContext.GetTokenAsync("access_token").Result;
            var types = _productsService.GetProductTypes(accessToken);
            TypeOptions = types.Select(type =>
                new SelectListItem
                {
                    Value = type,
                    Text = type
                }).ToList();
        }
        
        public IActionResult OnPost()
        {
            string accessToken = HttpContext.GetTokenAsync("access_token").Result;
            _productsService.Create(new ProductCreateInputModel()
            {
                Name = this.Name, 
                Type = this.Type, 
                Price = this.Price
            }, accessToken);

            return RedirectToPage("./Index");
        }
        
        public JsonResult OnPostCheckPrice()
        {
            bool valid = true;
            if (Price < 5)
            {
                valid = false;
            }
            return new JsonResult(valid);
        }
    }
}