using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VimServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentsController : ControllerBase
    {

        private readonly ILogger<AppointmentsController> _logger;

        public AppointmentsController()
        {
            //_logger = logger;
        }

        [HttpGet]
        public ActionResult Get([FromQuery] string specialty, [FromQuery] long date, [FromQuery] double minScore)
        {
            //TODO: move to DI
            var repository = new ProviderRepository();

            if (string.IsNullOrWhiteSpace(specialty))
                return BadRequest();
            if (date <= 0)
                return BadRequest();

            var results = repository.GetProviders()
                .Where(x => x.Score >= minScore) //Threshold;
                .Where(x => x.Specialties.Contains(specialty, StringComparer.OrdinalIgnoreCase)) //Specialty
                .Where(x => x.AvailableDates.Any(y => y.From <= date && y.To >= date)) //Availablity
                .OrderByDescending(x => x.Score) //Ordering
                .Select(x => x.Name)
                .ToList(); //result

            return Ok(results);
        }

        [HttpPost]
        public ActionResult Post([FromBody] AppointmentRequest request)
        {
            //TODO: move to DI
            var repository = new ProviderRepository();

            var date = request.Date;

            var canBook = repository.GetProviders()
                .Where(x => x.Name == request.Name)
                .Where(x => x.AvailableDates.Any(y => y.From <= date && y.To >= date))
                .Any();

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
