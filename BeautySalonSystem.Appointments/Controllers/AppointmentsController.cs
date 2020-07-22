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
        public IActionResult List([FromQuery] string customerId)
        {
            // if (string.IsNullOrWhiteSpace(customerId))
            // {
            //     return BadRequest("Customer id cannot be null or empty.");
            // }

            var appointments = _repository.GetConfirmedByCustomerId(customerId);
            return Ok(appointments);
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

        [HttpPost("{id}")]
        public IActionResult ConfirmAppointment(int id)
        {
            var appointment = _repository.GetByID(id);

            appointment.IsConfirmed = true;
            _repository.Update(appointment);
            _repository.SaveChanges();

            return Ok();
        }

    }
}