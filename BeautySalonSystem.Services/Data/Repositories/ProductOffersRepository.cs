using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BeautySalonSystem.Products.Data.Models;
using BeautySalonSystem.Services;

namespace BeautySalonSystem.Products.Data.Repositories
{
    public class ProductOffersRepository : IProductOffersRepository
    {
        private readonly ProductsDbContext _context;

        public ProductOffersRepository(ProductsDbContext context)
        {
            _context = context;
        }
        public void Add(ProductOffer item)
        {
            this._context.ProductOffers.Add(item);
        }

        public void Delete(ProductOffer item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductOffer> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<ProductOffer> GetAllByProductId(int productId)
        {
            return this._context.ProductOffers.Select(po => po).Where(po => po.ProductId == productId).ToList();
        }

        public ProductOffer GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
            var res = _context.SaveChanges();

            return Convert.ToBoolean(res);
        }

        public void Update(ProductOffer item)
        {
            throw new NotImplementedException();
        }
    }
}
