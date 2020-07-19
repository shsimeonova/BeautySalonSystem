﻿using System.Collections.Generic;
using BeautySalonSystem.UI.Models;
using BeautySalonSystem.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BeautySalonSystem.UI.Pages.Admin.Offers
{
    public class AdminOffersIndex : PageModel
    {
        private IOffersService offersService;

        public AdminOffersIndex(IOffersService offersService)
        {
            this.offersService = offersService;
        }

        public IEnumerable<ViewOfferOutputModel> AllOffers { get; set; }
        
        public void OnGet()
        {
            var result = offersService.GetAll();
            AllOffers = result;
        }
    }
}