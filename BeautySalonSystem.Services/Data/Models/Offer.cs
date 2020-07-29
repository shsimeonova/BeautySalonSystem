using BeautySalonSystem.Products.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySalonSystem.Products.Data.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal TotalPrice { get; set; }
        public int Discount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Image { get; set; }
        public IEnumerable<ProductOffer> ProductOffers { get; set; }
        public string AddedById { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
