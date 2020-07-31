using BeautySalonSystem.Products.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySalonSystem.Products.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public ProductType Type { get; set; }
        public IEnumerable<ProductOffer> ProductOffers { get; set; }
        [Required]
        public int Duration { get; set; }
        public bool IsDeleted { get; set; }
    }
}
