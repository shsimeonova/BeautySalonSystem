using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BeautySalonSystem.Controllers;
using BeautySalonSystem.Products.Data.Models;
using BeautySalonSystem.Products.Data.Repositories;
using BeautySalonSystem.Products.Models.ProductOffer;
using BeautySalonSystem.Products.Services;
using BeautySalonSystem.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySalonSystem.Products.Controllers
{
    public class ProductOffersController : ApiController
    {
        private readonly IProductOffersService _productOffersService;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public ProductOffersController(IProductOffersService productOffersService, IMapper mapper, ICurrentUserService currentUser)
        {
            this._productOffersService = productOffersService;
            this._mapper = mapper;
            this._currentUser = currentUser;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CreateProductOfferOutputModel>> Create(CreateProductOfferInputModel input)
        {
            return await _productOffersService.Create(input);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ListProductOffersModel>> ListByProductId([FromQuery(Name = "productId")] int productId)
        {
            return await _productOffersService.GetAllByProductId(productId);
        }
    }
}
