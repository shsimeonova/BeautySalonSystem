using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BeautySalonSystem.Controllers;
using BeautySalonSystem.Products.Models;
using BeautySalonSystem.Products.Models.ProductOffer;
using BeautySalonSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IOffersService = BeautySalonSystem.Products.Services.IOffersService;

namespace BeautySalonSystem.Products.Controllers
{
    public class OffersController : ApiController
    {
        private readonly IOffersService _offersService;
        private readonly IMapper _mapper;

        public OffersController(IOffersService offersService, IMapper mapper)
        {
            _offersService = offersService;
            _mapper = mapper;
        }

        [HttpPost]
        public CreateProductOfferOutputModel Create(CreateProductOfferInputModel input)
        {
            string currentUserId = ((ClaimsIdentity)User.Identity)
                .Claims
                .FirstOrDefault(c => c.Type == "sub")
                ?.Value;

            var offerId = _offersService.Create(input, currentUserId);
            return new CreateProductOfferOutputModel{ OfferId = offerId };
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<GetOfferModel> ListByProductId([FromQuery(Name = "productId")] int productId)
        {
            if (productId == 0)
            {
                var offers = _offersService.GetAll();
                return offers;
            }
            // return await _productOffersService.GetAllByProductId(productId);
            return null;
        }
    }
}
