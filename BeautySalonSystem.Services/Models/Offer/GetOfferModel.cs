using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySalonSystem.Products.Models
{
    public class GetOfferModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Discount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public IEnumerable<GetProductOutputModel> Products { get; set; }
        public bool IsActive { get; set; }
    }
}
