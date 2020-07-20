using System;
using BeautySalonSystem.Appointments.Data.Models;
using BeautySalonSystem.Appointments.Data.Repositories;
using BeautySalonSystem.Controllers;
using BeautySalonSystem.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySalonSystem.Appointments.Controllers
{
    
    public class AppointmentsController : ApiController
    {
        private IAppointmentsRepository _repository;

        public AppointmentsController(IAppointmentsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Authorize]
        public IActionResult List()
        {
            Console.WriteLine();
            return null;
        }
        
        [HttpPost]
        [Authorize]
        public IActionResult CreateAppointmentRequest(AppointmentCreateInputModel input)
        {
            var appointment = new Appointment()
            {
                CustomerId = input.CustomerId,
                OfferId = input.OfferId,
                Date = input.Date,
                IsConfirmed = false
            };
            
            _repository.Add(appointment);
            _repository.SaveChanges();

            return Ok(appointment.Id);
        }
    }
}