
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Authentication;
using BeautySalonSystem.UI.Models;
using BeautySalonSystem.UI.Services;
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
        
        [BindProperty, Required]
        public string Name { get; set; }
        [BindProperty, Required]
        public decimal Price { get; set; }
        [BindProperty, Required]
        public string Type { get; set; }
        
        public IEnumerable<SelectListItem> TypeOptions { get; set; }
        
        public void OnGet()
        {
            var types = _productsService.GetProductTypes(HttpContext.Request);
            TypeOptions = types.Select(type =>
                new SelectListItem
                {
                    Value = type,
                    Text = type
                }).ToList();
        }
        
        public IActionResult OnPost()
        {
            try
            {
                _productsService.Create(new ProductCreateInputModel()
                {
                    Name = this.Name, 
                    Type = this.Type, 
                    Price = this.Price
                }, HttpContext.Request);
            }
            catch (AuthenticationException ae)
            {
                
            }

            return RedirectToPage("./Index");
        }
    }
}