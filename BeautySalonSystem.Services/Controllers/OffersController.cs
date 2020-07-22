using System;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using BeautySalonSystem.Controllers;
using BeautySalonSystem.Products.Models.ProductOffer;
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

        [HttpGet]
        public IActionResult ListByProductId([FromQuery(Name = "id")] int id,
            [FromQuery(Name = "activeOnly")] bool activeOnly = false)
        {
            if (id == 0)
            {
                var offers = _offersService.GetAll(activeOnly);
                return Ok(offers);
            }
            
            var offer = _offersService.GetById(id);
            return Ok(offer);
        }

        [HttpGet("all")]
        public IActionResult GetManyByIds([FromQuery] int[] ids, [FromQuery] bool activeOnly)
        {
            if (ids.Length == 0)
            {
                return BadRequest("Offer ids array cannot be empty");
            }
            
            return Ok(_offersService.GetManyByIds(activeOnly, ids));
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
        
        [HttpPut("{id}")]
        public IActionResult Edit([FromBody] CreateProductOfferInputModel input, int id)
        {
            var offer = _offersService.GetById(id);

            if (offer == null)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            try
            {
                _offersService.Delete(id);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            return Ok();
        }
        
        [HttpGet("activate/{id}")]
        [Authorize]
        public IActionResult Activate(int id)
        {
            try
            {
                _offersService.Activate(id);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
