using System;
using BeautySalonSystem.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySalonSystem.Appointments.Controllers
{
    public class AppointmentsController : ApiController
    {
        public AppointmentsController() {}

        [HttpGet]
        [Authorize]
        public IActionResult List()
        {
            Console.WriteLine();
            return null;
        }
    }
}