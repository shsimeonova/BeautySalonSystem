using System;
using System.Collections.Generic;
using System.Linq;
using BeautySalonSystem.Products.Data;
using BeautySalonSystem.Products.Data.Models;
using BeautySalonSystem.Products;
using BeautySalonSystem.Products.Data.Repositories;
using System.Linq.Expressions;

namespace BeautySalonSystem.Products.Data
{
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

        public IEnumerable<Offer> GetAll()
        {
            throw new NotImplementedException();
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