using BeautySalonSystem.Products.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySalonSystem.Products.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ProductType Type { get; set; }
        public IEnumerable<ProductOffer> ProductOffers { get; set; }
        
        public int Duration { get; set; }
        public bool IsDeleted { get; set; }
    }
}
