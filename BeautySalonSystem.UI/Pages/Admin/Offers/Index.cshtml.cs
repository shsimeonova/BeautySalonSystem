using System;
using System.Collections.Generic;
using BeautySalonSystem.UI.Models;
using BeautySalonSystem.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeautySalonSystem.UI.Pages.Admin.Offers
{
    public class AdminOffersIndex : PageModel
    {
        private IOffersService _offersService;

        public AdminOffersIndex(IOffersService offersService)
        {
            this._offersService = offersService;
        }

        public IEnumerable<OfferViewModel> AllOffers { get; set; }
        
        public void OnGet()
        {
            var result = _offersService.GetAll(false);
            AllOffers = result;
        }

        public void OnPostDeleteOffer(int id)
        {
            _offersService.Delete(id, HttpContext.GetTokenAsync("access_token").Result);
        }
        
        public void OnPostActivateOffer(int id)
        {
            string accessToken = HttpContext.GetTokenAsync("access_token").Result;
            _offersService.Activate(id, accessToken);
            OnGet();
        }
    }
}