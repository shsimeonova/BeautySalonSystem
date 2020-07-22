using AutoMapper;
using BeautySalonSystem.Controllers;
using BeautySalonSystem.Products.Data.Models;
using BeautySalonSystem.Products.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySalonSystem.Products.Data
{
    public class ProductsController : ApiController
    {
        private readonly IProductsRepository _repository;
        private readonly IMapper _mapper;

        public ProductsController(IProductsRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult List()
        {
            var products = _repository.GetAll();

            return Ok(_mapper.ProjectTo<GetProductOutputModel>(products.AsQueryable<Product>()));
        }
        
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            var product = _repository.GetByID(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<GetProductOutputModel>(product));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] CreateProductInputModel input)
        {
            var product = new Product
            {
                Name = input.Name,
                Price = input.Price,
                Type = (ProductType)Enum.Parse(typeof(ProductType), input.Type),
                ProductOffers = new List<ProductOffer>(),
                Duration = input.Duration
            };

            _repository.Add(product);
            _repository.SaveChanges();

            return Ok(product.Id);
        }
        
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Edit(int id, [FromBody] CreateProductInputModel input)
        {
            var product = _repository.GetByID(id);

            if (product == null)
            {
                return NotFound();
            }

            product.Name = input.Name;
            product.Price = input.Price;
            product.Type = (ProductType) Enum.Parse(typeof(ProductType), input.Type);

            _repository.Update(product);
            _repository.SaveChanges();

            return Ok(product.Id);
        }
        
        [HttpDelete]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var product = _repository.GetByID(id);

            if (product == null)
            {
                return NotFound();
            }

            _repository.Delete(product);
            _repository.SaveChanges();

            return Ok();
        }
        
        [HttpGet("types")]
        [Authorize]
        public IActionResult GetProductOptions()
        {
            var types = Enum.GetNames(typeof(ProductType)).ToList();

            return Ok(types);
        }
    }
}
