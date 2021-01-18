using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VimServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentsController : ControllerBase
    {

        private readonly IAppointmentsService _appointmentsService;

        public AppointmentsController(IAppointmentsService appointmentsService)
        {
            _appointmentsService = appointmentsService;
        }

        [HttpGet]
        public ActionResult Get([FromQuery] string specialty, [FromQuery] long date, [FromQuery] double minScore)
        {
            if (string.IsNullOrWhiteSpace(specialty))
                return BadRequest();
            if (date <= 0)
                return BadRequest();
            if (minScore < 0 || minScore > 10)
                return BadRequest();

            var results = _appointmentsService.GetAvailableProviders(specialty, date, minScore);

            return Ok(results);
        }

        [HttpPost]
        public ActionResult Post([FromBody] AppointmentRequest request)
        {
            var date = request.Date;

            var canBook = _appointmentsService.TryBookAppointment(request.Name, request.Date);

            if (!canBook)
            {
                return BadRequest();
            }

            return Ok();
        }
    }

    public class AppointmentRequest
    {
        public string Name { get; set; }
        public long Date { get; set; }
    }
}
