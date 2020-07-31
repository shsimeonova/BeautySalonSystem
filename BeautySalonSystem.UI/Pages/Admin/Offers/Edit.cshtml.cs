using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BeautySalonSystem.UI.Models;
using BeautySalonSystem.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeautySalonSystem.UI.Pages.Admin.Offers
{
    public class Edit : PageModel
    {
        private IOffersService _offersService;
        
        public Edit(IOffersService offersService)
        {
            _offersService = offersService;
        }
        public OfferViewModel OfferViewModel { get; set; }
        
        [BindProperty]
        [Required, MinLength(3)]
        public string Name { get; set; }
        
        [BindProperty, Required]
        [PageRemote(
            ErrorMessage ="Датата на валидност не може да бъде в миналото.",
            AdditionalFields = "__RequestVerificationToken", 
            HttpMethod ="post",
            PageName = "./Create",
            PageHandler ="CheckExpiryDateIsBeforeNow"
        )]
        public string ExpiryDate { get; set; }
        
        [BindProperty]
        public decimal TotalPrice { get; set; }
        
        [BindProperty]
        [Range(0, 90)]
        public int Discount { get; set; }
        
        [BindProperty]
        public IEnumerable<int> SelectedProductsIds { get; set; }
        
        [BindProperty]
        public IEnumerable<SelectListItem> Products { get; set; }    
        
        public void OnGet(int id)
        {
            OfferViewModel = _offersService.GetById(id);
            Products = OfferViewModel.Products.Select(product =>
                new SelectListItem
                {
                    Value = product.Id.ToString(),
                    Text = product.Name
                }).ToList();
            SelectedProductsIds = OfferViewModel.Products.Select(p => p.Id);
            Console.WriteLine();
        }

        public void OnPost()
        {
            Console.WriteLine();
        }
    }
}