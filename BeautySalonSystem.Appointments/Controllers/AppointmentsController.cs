using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            var appointments = _repository.GetAll();

            return Ok(appointments);
        }
        
        [HttpGet("confirmed")]
        [Authorize]
        public IActionResult ListConfirmed()
        {
            var appointments = _repository.GetAllConfirmed();

            return Ok(appointments);
        }
        
        [HttpGet("non-confirmed")]
        [Authorize]
        public IActionResult ListNonConfirmed()
        {
            var appointments = _repository.GetAllNonConfirmed();

            return Ok(appointments);
        }
        
        [HttpGet("customers/{customerId}")]
        [Authorize]
        public IActionResult List(string customerId)
        {
            var appointments = _repository.GetByCustomerId(customerId);

            return Ok(appointments);
        }
        
        [HttpGet("customers/{customerId}/confirmed")]
        [Authorize]
        public IActionResult ListConfirmed(string customerId)
        {
            var appointments = _repository.GetByCustomerIdConfirmed(customerId);

            return Ok(appointments);
        }
        
        [HttpGet("customers/{customerId}/non-confirmed")]
        [Authorize]
        public IActionResult ListNonConfirmed(string customerId)
        {
            var appointments = _repository.GetByCustomerIdNonConfirmed(customerId);

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

        [HttpPost("{id}/confirm")]
        public IActionResult ConfirmAppointment(int id)
        {
            var appointment = _repository.GetByID(id);

            appointment.IsConfirmed = true;
            _repository.Update(appointment);
            _repository.SaveChanges();

            return Ok();
        }

        [HttpPost("check-time")]
        public IActionResult CheckIsAppointmentRequestTimeFree(CheckAppointmentRequestTimeInputModel input)
        {
            var appointments = _repository.GetAll();
            var closestAppointment = appointments
                .OrderBy(a => Math.Abs((a.Date - input.AppointmentRequestTime).Ticks))
                .First();

            double difference = Math.Abs(input.AppointmentRequestTime.Subtract(closestAppointment.Date).TotalMinutes);

            if (difference < input.AppointmentRequestDuration)
            {
                return Ok(false);
            }
            
            return Ok(true);
        }
        
    }
}