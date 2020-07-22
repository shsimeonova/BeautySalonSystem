using System;
using System.Collections.Generic;
using System.Linq;
using BeautySalonSystem.Appointments.Data.Models;

namespace BeautySalonSystem.Appointments.Data.Repositories
{
    public interface IAppointmentsRepository
    {
        void Add(Appointment item);

        IEnumerable<AppointmentDto> GetAll();

        Appointment GetByID(int id);
        
        IEnumerable<Appointment> GetConfirmedByCustomerId(string customerId);

        void Update(Appointment item);

        bool SaveChanges();
    }
    
    public class AppointmentsRepository : IAppointmentsRepository
    {
        private readonly AppointmentsDbContext _context;

        public AppointmentsRepository(AppointmentsDbContext context)
        {
            _context = context;
        }
        
        public void Add(Appointment item)
        {
            if (item == null) 
            {
                throw new ArgumentNullException();
            }
            
            _context.Appointments.Add(item);
        }

        public IEnumerable<AppointmentDto> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Appointment GetByID(int id)
        {
            var result = _context.Appointments.Find(id);
            return result;
        }

        public IEnumerable<Appointment> GetConfirmedByCustomerId(string customerId)
        {
            var query = _context.Appointments.Where(a => a.CustomerId == customerId);
            query = query.Where(a => a.IsConfirmed);
            
            return query.ToList();
        }

        public void Update(Appointment item)
        {
            _context.Update(item);
        }

        public bool SaveChanges()
        {
            var res = _context.SaveChanges();
            return Convert.ToBoolean(res);
        }
    }
}