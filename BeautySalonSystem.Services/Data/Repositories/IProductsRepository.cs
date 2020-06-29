using BeautySalonSystem.Products.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySalonSystem.Products.Data.Repositories
{
    public interface IProductsRepository
    {
        void Add(Product item);

        void Delete(Product item);

        IEnumerable<Product> GetAll();

        Product GetByID(int id);

        void Update(Product item);

        bool SaveChanges();
    }
}
