using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using BeautySalonSystem.UI.Models;
using BeautySalonSystem.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BeautySalonSystem.UI.Pages.Admin.Products
{
    [Authorize(Policy = "Admin")]
    public class ProductsIndex : PageModel
    {
        private IProductsService _productsService;
        
        public ProductsIndex(IProductsService productsService)
        {
            _productsService = productsService;
        }

        public IEnumerable<ProductViewModel> AllProducts { get; set; }
        
        public async Task<IActionResult> OnGet()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            AllProducts = _productsService.GetAll(accessToken);
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteProduct(int id)
        {
            string accessToken = HttpContext.GetTokenAsync("access_token").Result;
            _productsService.Delete(id, accessToken);
            // return new RedirectToPageResult("/Users/Login", new {returnUrl = "/Admin/Products?handler=PostDeleteProduct"});
            return await this.OnGet();
        }
    }
}