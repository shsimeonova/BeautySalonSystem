using System.Collections.Generic;
using System.Threading.Tasks;
using BeautySalonSystem.UI.Models;
using BeautySalonSystem.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
            AllProducts = _productsService.GetAll();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteProduct(int id)
        {
            _productsService.Delete(id);
            return await OnGet();
        }
    }
}