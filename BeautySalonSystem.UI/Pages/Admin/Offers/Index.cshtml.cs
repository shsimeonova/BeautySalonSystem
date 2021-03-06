﻿using System.Collections.Generic;
using BeautySalonSystem.UI.Models;
using BeautySalonSystem.UI.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeautySalonSystem.UI.Pages.Admin.Offers
{
    public class AdminOffersIndex : PageModel
    {
        private IOffersService _offersService;

        public AdminOffersIndex(IOffersService offersService)
        {
            _offersService = offersService;
        }

        public IEnumerable<OfferViewModel> AllOffers { get; set; }
        
        public void OnGet()
        {
            var result = _offersService.GetAll(false);
            AllOffers = result;
        }

        public void OnPostDeleteOffer(int id)
        {
            _offersService.Delete(id);
        }
        
        public void OnPostActivateOffer(int id)
        {
            _offersService.Activate(id);
            OnGet();
        }
    }
}