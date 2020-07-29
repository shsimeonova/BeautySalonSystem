using System;
using System.Collections.Generic;
using System.Linq;
using BeautySalonSystem.Products.Data;
using BeautySalonSystem.Products.Data.Models;
using BeautySalonSystem.Products;
using BeautySalonSystem.Products.Data.Repositories;
using System.Linq.Expressions;
using BeautySalonSystem.Products.Models;
using Microsoft.EntityFrameworkCore;

namespace BeautySalonSystem.Products.Data
{
    public interface IOffersRepository
    {
        void Add(Offer item);

        void Delete(Offer item);

        IEnumerable<OfferDto> GetAll(bool activeOnly);
        IEnumerable<OfferDto> GetAllByIds(bool activeOnly, int[] ids);
        Offer GetById(int id);
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
            item.IsDeleted = true;
            Update(item);
        }

        public IEnumerable<OfferDto> GetAll(bool activeOnly)
        {
            var query=  _context.Offers.Where(o => !o.IsDeleted);
            
            if (activeOnly)
            {
                query = query.Where(o => o.IsActive);
            }

            return query.Select(o => new OfferDto
            {
                Id = o.Id,
                Name = o.Name,
                Discount = o.Discount,
                TotalPrice = o.TotalPrice,
                ExpiryDate = o.ExpiryDate,
                ImageUrl = o.Image,
                AddedById = o.AddedById,
                Products = o.ProductOffers.Select(po => po.Product).ToList(),
                IsActive = o.IsActive
            }).ToList();
        }

        public IEnumerable<OfferDto> GetAllByIds(bool activeOnly, int[] ids)
        {
            var query=  _context.Offers
                .Where(o => !o.IsDeleted)
                .Where(o => ids.Contains(o.Id));
            
            if (activeOnly)
            {
                query = query.Where(o => o.IsActive).Where(o => o.ExpiryDate > DateTime.Now);
            }
            
            return query.Select(o => new OfferDto
            {
                Id = o.Id,
                Name = o.Name,
                Discount = o.Discount,
                TotalPrice = o.TotalPrice,
                ExpiryDate = o.ExpiryDate,
                AddedById = o.AddedById,
                Products = o.ProductOffers.Select(po => po.Product).ToList(),
                IsActive = o.IsActive
            }).ToList();
        }

        public Offer GetById(int id)
        {
            var offer = _context.Offers
                .Include(o => o.ProductOffers)
                .ThenInclude(po => po.Product)
                .Where(o => o.Id == id).FirstOrDefault();

            return offer;
        }

        public bool SaveChanges()
        {
            var res = _context.SaveChanges();

            return Convert.ToBoolean(res);
        }

        public void Update(Offer item)
        {
            _context.Offers.Update(item);
        }
    }
}