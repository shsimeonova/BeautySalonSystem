using System;
using System.Collections.Generic;
using BeautySalonSystem.Appointments.Data.Models;

namespace BeautySalonSystem.Appointments.Data.Repositories
{
    public interface IAppointmentsRepository
    {
        void Add(Appointment item);

        void Delete(Appointment item);

        IEnumerable<AppointmentDto> GetAll();

        Appointment GetByID(int id);

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

        public void Delete(Appointment item)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<AppointmentDto> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Appointment GetByID(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Appointment item)
        {
            throw new System.NotImplementedException();
        }

        public bool SaveChanges()
        {
            var res = _context.SaveChanges();
            return Convert.ToBoolean(res);
        }
    }
}