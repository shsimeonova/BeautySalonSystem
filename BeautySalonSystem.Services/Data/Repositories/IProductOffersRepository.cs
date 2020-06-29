using BeautySalonSystem.Products.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
}
