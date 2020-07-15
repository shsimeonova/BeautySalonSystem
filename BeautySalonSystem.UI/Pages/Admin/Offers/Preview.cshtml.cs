using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeautySalonSystem.UI.Models;
using BeautySalonSystem.UI.Services;
using BeautySalonSystem.UI.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeautySalonSystem.UI.Pages.Admin.Offers
{
    [Authorize(Policy = "Admin")]
    public class Preview : PageModel
    {
        private IOffersService offersService;
        private ISessionHelper _sessionHelper;

        public Preview(IOffersService offersService, ISessionHelper sessionHelper)
        {
            this.offersService = offersService;
            _sessionHelper = sessionHelper;
        }
        
        [BindProperty]
        public IEnumerable<SelectListItem> Products { get; set; }

        [BindProperty]
        public IEnumerable<int> SelectedProductsIds { get; set; }
        
        [BindProperty]
        public CreateOfferInputModel CreateOfferInput { get; set; }
        
        public void OnGet()
        {
            CreateOfferInput = (CreateOfferInputModel) _sessionHelper.GetItem("CreateOfferInput");
            Products = ((IEnumerable<ProductViewModel>) _sessionHelper.GetItem("AllProducts"))
                .Select(product =>
                    new SelectListItem
                    {
                        Value = product.Id.ToString(),
                        Text = product.Name
                    })
                .ToList();;
        }

        public async Task<IActionResult> OnPost()
        {
            var access_token = await HttpContext.GetTokenAsync("access_token");
            offersService.Create(CreateOfferInput, access_token);
            
            return RedirectToPage("./Index");
        }
    }
}