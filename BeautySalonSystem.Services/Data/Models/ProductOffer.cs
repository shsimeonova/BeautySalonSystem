using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySalonSystem.Products.Data.Models
{
    public class ProductOffer
    {
        public int ProductId { get; set; }
        public int OfferId { get; set; }
        public Product Product { get; set; }
        public Offer Offer { get; set; }
        
        public bool IsDeleted { get; set; }
    }
}
