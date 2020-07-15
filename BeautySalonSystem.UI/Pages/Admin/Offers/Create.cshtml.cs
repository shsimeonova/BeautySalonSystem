using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using BeautySalonSystem.UI.Models;
using BeautySalonSystem.UI.Services;
using BeautySalonSystem.UI.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace BeautySalonSystem.UI.Pages.Admin.Offers
{
    [Authorize(Policy = "Admin")]
    public class Create : PageModel
    {
        private IProductsService _productsService;
        private ISessionHelper _sessionHelper;
        
        public Create(IProductsService productsService, ISessionHelper sessionHelper)
        {
            _productsService = productsService;
            _sessionHelper = sessionHelper;
        }

        [BindProperty, Required]
        public string Name { get; set; }
        
        [BindProperty, Required]
        public string ExpiryDate { get; set; }
        
        [BindProperty]
        public IEnumerable<int> SelectedProductsIds { get; set; }
        
        [BindProperty]
        public IEnumerable<SelectListItem> Products { get; set; }
        
        public async Task<IActionResult> OnGet()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            IEnumerable<ProductViewModel> allProducts = _productsService.GetAll(accessToken);
            _sessionHelper.AddRenewItem("AllProducts", allProducts);;
            Products = allProducts.Select(product =>
                new SelectListItem
                {
                    Value = product.Id.ToString(),
                    Text = product.Name
                }).ToList();

            return Page();
        }
        
        public IActionResult OnPost()
        {
            IEnumerable<ProductViewModel> allProducts = (IEnumerable<ProductViewModel>)_sessionHelper.GetItem("AllProducts");
            var createOfferInput = new CreateOfferInputModel
            {
                Name = this.Name,
                ProductIds = this.SelectedProductsIds.ToArray(),
                TotalPrice = allProducts
                                .Where(vm => SelectedProductsIds.Contains(vm.Id))
                                .Sum(p => p.Price),
                ExpiryDate = this.ExpiryDate
            };
            
            _sessionHelper.AddRenewItem("CreateOfferInput", createOfferInput);
            return RedirectToPage("./Preview");
        }
    }
}