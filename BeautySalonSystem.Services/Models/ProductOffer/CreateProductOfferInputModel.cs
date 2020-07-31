using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySalonSystem.Products.Models.ProductOffer
{
    public class CreateProductOfferInputModel
    {
        public IEnumerable<int> Products { get; set; }

        public string Name { get; set; }

        public decimal TotalPrice { get; set; }

        public int Discount { get; set; }

        public string ExpiryDate { get; set; }
        
        public string ImageUrl { get; set; }
        
        public bool IsActive { get; set; }
    }
}
