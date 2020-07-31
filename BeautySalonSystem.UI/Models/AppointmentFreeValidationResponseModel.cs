using System;

namespace BeautySalonSystem.UI.Models
{
    public class AppointmentFreeValidationResponseModel
    {
        public bool IsRequestTimeFree { get; set; }
        public int ClosestAppointmentId { get; set; }
        public DateTime ClosestAppointmentDate { get; set; }
        public int ClosestAppointmentOfferId { get; set; }    
    }
}