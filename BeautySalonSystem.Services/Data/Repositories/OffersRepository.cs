using System;
using System.Collections.Generic;
using System.Linq;
using BeautySalonSystem.Products.Data;
using BeautySalonSystem.Products.Data.Models;
using BeautySalonSystem.Products;
using BeautySalonSystem.Products.Data.Repositories;
using System.Linq.Expressions;
using BeautySalonSystem.Products.Models;

namespace BeautySalonSystem.Products.Data
{
    public interface IOffersRepository
    {
        void Add(Offer item);

        void Delete(Offer item);

        IEnumerable<OfferDto> GetAll();

        Offer GetByID(int id);

        void Update(Offer item);

        bool SaveChanges();
    }
    
    public class OffersRepository : IOffersRepository
    {
        private readonly ProductsDbContext _context;
        
        public OffersRepository(ProductsDbContext context)
        {
            _context = context;
        }

        public void Add(Offer item)
        {
            this._context.Offers.Add(item);
        }

        public void Delete(Offer item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OfferDto> GetAll()
        {
            return _context.Offers.Select(o => new OfferDto
            {
                Id = o.Id,
                Name = o.Name,
                Discount = o.Discount,
                TotalPrice = o.TotalPrice,
                ExpiryDate = o.ExpiryDate,
                AddedById = o.AddedById,
                Products = o.ProductOffers.Select(po => po.Product).ToList()
            }).ToList();
        }

        public Offer GetByID(int id)
        {
            return this._context.Offers.Find(id);
        }

        public bool SaveChanges()
        {
            var res = _context.SaveChanges();

            return Convert.ToBoolean(res);
        }

        public void Update(Offer item)
        {
            throw new NotImplementedException();
        }
    }
}