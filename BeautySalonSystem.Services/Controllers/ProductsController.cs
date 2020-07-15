using AutoMapper;
using BeautySalonSystem.Controllers;
using BeautySalonSystem.Products.Data.Models;
using BeautySalonSystem.Products.Data.Repositories;
using BeautySalonSystem.Products.Models;
using BeautySalonSystem.Services.Identity;
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
        [Authorize]
        public async Task<IEnumerable<GetProductOutputModel>> List()
        {
            var products = _repository.GetAll();

            return this._mapper.ProjectTo<GetProductOutputModel>(products.AsQueryable<Product>());
        }
        
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<GetProductOutputModel>> GetById(int id)
        {
            var product = _repository.GetByID(id);

            if (product == null)
            {
                return NotFound();
            }

            return this._mapper.Map<GetProductOutputModel>(product);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<int>> Create([FromBody] CreateProductInputModel input)
        {
            var product = new Product
            {
                Name = input.Name,
                Price = input.Price,
                Type = (ProductType)Enum.Parse(typeof(ProductType), input.Type),
                ProductOffers = new List<ProductOffer>()
            };

            this._repository.Add(product);
            this._repository.SaveChanges();

            return product.Id;
        }
        
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<int>> Edit(int id, [FromBody] CreateProductInputModel input)
        {
            var product = this._repository.GetByID(id);

            if (product == null)
            {
                return NotFound();
            }

            product.Name = input.Name;
            product.Price = input.Price;
            product.Type = (ProductType) Enum.Parse(typeof(ProductType), input.Type);

            _repository.Update(product);
            _repository.SaveChanges();

            return product.Id;
        }
        
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var product = _repository.GetByID(id);

            if (product == null)
            {
                return NotFound();
            }

            _repository.Delete(product);
            _repository.SaveChanges();

            return true;
        }
        
        [HttpGet("types")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<string>>> GetProductOptions()
        {
            var types = Enum.GetNames(typeof(ProductType)).ToList();

            return types;
        }
    }
}
