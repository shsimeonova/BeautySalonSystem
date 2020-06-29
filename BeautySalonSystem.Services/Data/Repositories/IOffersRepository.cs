using BeautySalonSystem.Products.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySalonSystem.Products.Data.Repositories
{
    public interface IOffersRepository
    {
        void Add(Offer item);

        void Delete(Offer item);

        IEnumerable<Offer> GetAll();

        Offer GetByID(int id);

        void Update(Offer item);

        bool SaveChanges();
    }
}
