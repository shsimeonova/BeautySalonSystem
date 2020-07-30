using System;

namespace BeautySalonSystem.Messages
{
    public class AppointmentConfirmedMessage
    {
        public string CustomerEmail { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string OfferName { get; set; }
    }
}