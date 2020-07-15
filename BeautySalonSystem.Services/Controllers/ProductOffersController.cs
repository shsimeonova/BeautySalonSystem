using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BeautySalonSystem.Controllers;
using BeautySalonSystem.Products.Models.ProductOffer;
using BeautySalonSystem.Products.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySalonSystem.Products.Controllers
{
    public class ProductOffersController : ApiController
    {
        private readonly IProductOffersService _productOffersService;
        private readonly IMapper _mapper;

        public ProductOffersController(IProductOffersService productOffersService, IMapper mapper)
        {
            this._productOffersService = productOffersService;
            this._mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<CreateProductOfferOutputModel>> Create(CreateProductOfferInputModel input)
        {
            return await _productOffersService.Create(input);
        }
        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ListProductOffersModel>>> List()
        {
            return await _productOffersService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ListProductOffersModel>> ListByProductId([FromQuery(Name = "productId")] int productId)
        {
            return await _productOffersService.GetAllByProductId(productId);
        }
    }
}
