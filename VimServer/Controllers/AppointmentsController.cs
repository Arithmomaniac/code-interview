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

        private readonly IProviderRepository _providerRepository;

        public AppointmentsController(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }

        [HttpGet]
        public ActionResult Get([FromQuery] string specialty, [FromQuery] long date, [FromQuery] double minScore)
        {
            if (string.IsNullOrWhiteSpace(specialty))
                return BadRequest();
            if (date <= 0)
                return BadRequest();

            var results = _providerRepository.GetProviders()
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
            var date = request.Date;

            var canBook = _providerRepository.GetProviders()
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
