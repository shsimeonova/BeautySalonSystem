using System;
using System.Collections.Generic;
using BeautySalonSystem.Products.Data.Models;

namespace BeautySalonSystem.Products.Models
{
    public class OfferDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Discount { get; set; }
        public string AddedById { get; set; }
        public DateTime ExpiryDate { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public bool IsActive { get; set; }
    }
}