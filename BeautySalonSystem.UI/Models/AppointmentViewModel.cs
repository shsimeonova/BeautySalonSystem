using System;

namespace BeautySalonSystem.UI.Models
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateRequested { get; set; }
        public int offerId { get; set; }
        
        public string customerId { get; set; }
        
        public bool IsConfirmed { get; set; }
        
        public OfferViewModel offer;
    }
}