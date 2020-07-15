using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySalonSystem.Appointments.Data.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public string CustomerId { get; set; }

        public DateTime Date { get; set; }

        public string ProductOfferId { get; set; }
    }
}
