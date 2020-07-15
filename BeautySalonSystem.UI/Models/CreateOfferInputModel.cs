using System.Collections;
using System.Collections.Generic;

namespace BeautySalonSystem.UI.Models
{
    public class CreateOfferInputModel
    {
        public string Name { get; set; }
        public int[] ProductIds { get; set; }
        public int Discount { get; set; }
        public decimal TotalPrice { get; set; }
        
        public string ExpiryDate { get; set; }
    }
}