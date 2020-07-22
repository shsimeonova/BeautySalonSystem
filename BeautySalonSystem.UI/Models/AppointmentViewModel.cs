using System;
using System.Collections;

namespace BeautySalonSystem.UI.Models
{
    public class AppointmentViewModel
    {
        public DateTime Date { get; set; }

        public int offerId { get; set; }

        public OfferViewModel offer;
    }
}