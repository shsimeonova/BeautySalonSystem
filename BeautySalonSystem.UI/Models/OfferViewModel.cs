using System.Collections.Generic;

namespace BeautySalonSystem.UI.Models
{
    public class OfferViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Discount { get; set; }
        public string ExpiryDate { get; set; }
        public List<ProductViewModel> Products { get; set; }
        public bool IsActive { get; set; }
    }
}