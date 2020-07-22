using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public OfferCreateInputModel OfferCreateInput { get; set; }
        
        [BindProperty]
        [Required, MinLength(3)]
        public string Name { get; set; }

        [BindProperty]
        public decimal TotalPrice { get; set; }
        
        [BindProperty, Required]
        [PageRemote(
            ErrorMessage ="Датата на валидност не може да бъде в миналото.",
            AdditionalFields = "__RequestVerificationToken",
            PageName = "./Create",
            HttpMethod ="post",  
            PageHandler ="CheckExpiryDateIsBeforeNow"
        )]
        public string ExpiryDate { get; set; }
        
        [BindProperty]
        [Range(0, 90)]
        public int Discount { get; set; }
        
        public void OnGet()
        {
            OfferCreateInput = (OfferCreateInputModel) _sessionHelper.GetItem("CreateOfferInput");
            Name = OfferCreateInput.Name;
            TotalPrice = OfferCreateInput.TotalPrice;
            ExpiryDate = OfferCreateInput.ExpiryDate;
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
            OfferCreateInput.Name = Name;
            OfferCreateInput.Discount = Discount;
            OfferCreateInput.ExpiryDate = ExpiryDate;
            OfferCreateInput.TotalPrice = decimal.Round(TotalPrice, 2);
            offersService.Create(OfferCreateInput, access_token);
            
            return RedirectToPage("./Index");
        }
    }
}