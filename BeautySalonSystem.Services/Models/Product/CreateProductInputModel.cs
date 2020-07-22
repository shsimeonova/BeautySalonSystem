using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySalonSystem.Products.Models
{
    public class CreateProductInputModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public String Type { get; set; }
        
        public int Duration { get; set; }
    }
}
