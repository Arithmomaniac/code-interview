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
        public ActionResult Get([FromQuery] AppointmentsQuery appointmentsQuery)
        {
            var results = _appointmentsService.GetAvailableProviders(appointmentsQuery.Specialty, appointmentsQuery.Date, appointmentsQuery.MinScore);

            return Ok(results);
        }

        [HttpPost]
        public ActionResult Post([FromBody] AppointmentRequest request)
        {
            var canBook = _appointmentsService.TryBookAppointment(request.Name, request.Date);

            if (!canBook)
            {
                return BadRequest();
            }

            return Ok();
        }
    }

    public class AppointmentsQuery
    {
        [Required]
        public string Specialty { get; set; }
        [Range(0, long.MaxValue)]
        public long Date { get; set; }
        [Range(0, 10)]
        public double MinScore { get; set; }
    }

    public class AppointmentRequest
    {
        [Required]
        public string Name { get; set; }
        [Range(0, long.MaxValue)]
        public long Date { get; set; }
    }
}
