using System;
using System.Collections.Generic;
using System.Linq;
using BeautySalonSystem.Products.Data.Models;

namespace BeautySalonSystem.Products.Data.Repositories
{
    public interface IProductOffersRepository
    {
        void Add(ProductOffer item);

        void Delete(ProductOffer item);

        IEnumerable<ProductOffer> GetAll();

        ProductOffer GetByID(int id);

        void Update(ProductOffer item);

        List<ProductOffer> GetAllByProductId(int productId);

        bool SaveChanges();
    }
    
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
            item.IsDeleted = true;
            Update(item);
        }

        public IEnumerable<ProductOffer> GetAll()
        {
            return _context.ProductOffers.ToList();
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
            _context.ProductOffers.Update(item);
        }
    }
}
