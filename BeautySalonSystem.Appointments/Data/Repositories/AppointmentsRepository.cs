using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BeautySalonSystem.Appointments.Data.Models;

namespace BeautySalonSystem.Appointments.Data.Repositories
{
    public interface IAppointmentsRepository
    {
        void Add(Appointment item);
        IEnumerable<Appointment> GetAll();
        IEnumerable<Appointment> GetAllConfirmed();
        IEnumerable<Appointment> GetAllNonConfirmed();
        Appointment GetByID(int id);
        IEnumerable<Appointment> GetByCustomerId(string customerId);
        IEnumerable<Appointment> GetByCustomerIdConfirmed(string customerId);
        IEnumerable<Appointment> GetByCustomerIdNonConfirmed(string customerId);
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

        public IEnumerable<Appointment> GetAll()
        {
            return _context.Appointments.ToList();
        }
        
        public IEnumerable<Appointment> GetAllConfirmed()
        {
            var query = _context.Appointments.Select(a => a).Where(a => a.IsConfirmed);

            return query.ToList();
        }
        
        public IEnumerable<Appointment> GetAllNonConfirmed()
        {
            var query = _context.Appointments.Select(a => a).Where(a => !a.IsConfirmed);

            return query.ToList();
        }

        public Appointment GetByID(int id)
        {
            var result = _context.Appointments.Find(id);
            return result;
        }

        public IEnumerable<Appointment> GetByCustomerId(string customerId)
        {
            var query = _context.Appointments
                .Where(a => a.CustomerId == customerId);

            return query.ToList();
        }
        public IEnumerable<Appointment> GetByCustomerIdConfirmed(string customerId)
        {
            var query = _context.Appointments
                .Where(a => a.CustomerId == customerId)
                .Where(a => a.IsConfirmed);

            return query.ToList();
        }

        public IEnumerable<Appointment> GetByCustomerIdNonConfirmed(string customerId)
        {
            var query = _context.Appointments
                .Where(a => a.CustomerId == customerId)
                .Where(a => !a.IsConfirmed);

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