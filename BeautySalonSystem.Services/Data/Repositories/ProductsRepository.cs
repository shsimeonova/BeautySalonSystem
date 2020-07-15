using System;
using System.Collections.Generic;
using System.Linq;
using BeautySalonSystem.Products.Data;
using BeautySalonSystem.Products.Data.Models;
using System.Linq.Expressions;
using BeautySalonSystem.Services;
using BeautySalonSystem.Products.Data.Repositories;

namespace BeautySalonSystem.Products.Data
{
    public interface IProductsRepository
    {
        void Add(Product item);

        void Delete(Product item);

        IEnumerable<Product> GetAll();

        IEnumerable<Product> GetByIds(IEnumerable<int> ids);

        Product GetByID(int id);

        void Update(Product item);

        bool SaveChanges();
    }
    
    public class ProductsRepository : IProductsRepository
    {
        private readonly ProductsDbContext _context;
        
        public ProductsRepository(ProductsDbContext context)
        {
            _context = context;
        }
        
        public IEnumerable<Product> GetAll()
        {
            var products = _context.Products;

            return products.ToList();
        }


        public Product GetByID(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }
        
        public IEnumerable<Product> GetByIds(IEnumerable<int> ids)
        {
            return _context.Products.Where(product => ids.Contains(product.Id)).ToList();;
        }

        public void Add(Product item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }

            _context.Products.Add(item);
        }

        public void Update(Product item)
        {
            _context.Products.Update(item);
        }

        public void Delete(Product item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }
            _context.Products.Remove(item);
        }

        public bool SaveChanges()
        {
            var res = _context.SaveChanges();

            return Convert.ToBoolean(res);
        }
    }
}