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
        public async Task<IEnumerable<GetProductOutputModel>> List()
        {
            var products = _repository.GetAll();

            return this._mapper.ProjectTo<GetProductOutputModel>(products.AsQueryable<Product>());
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<int>> Create(CreateProductInputModel input)
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
    }
}
