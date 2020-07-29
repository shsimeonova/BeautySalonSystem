using System;

namespace BeautySalonSystem.UI.Models
{
    public class AppointmentCreateInputModel
    {
        public string CustomerId { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateRequested { get; set; }
        public int OfferId { get; set; }
    }
}