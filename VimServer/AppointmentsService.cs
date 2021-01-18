using System;
using System.Collections.Generic;
using System.Linq;

namespace VimServer
{
    public class AppointmentsService : IAppointmentsService
    {
        private readonly IProviderRepository _providerRepository;

        public AppointmentsService(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }

        public IEnumerable<string> GetAvailableProviders(string specialty, long date, double minScore)
        {
            return _providerRepository.GetProviders()
                .Where(x => x.Score >= minScore) //Threshold;
                .Where(x => x.Specialties.Contains(specialty, StringComparer.OrdinalIgnoreCase)) //Specialty
                .Where(x => x.AvailableDates.Any(y => y.From <= date && y.To >= date)) //Availablity
                .OrderByDescending(x => x.Score) //Ordering
                .Select(x => x.Name);
        }

        public bool TryBookAppointment(string name, long date)
        {
            return _providerRepository.GetProviders()
                .Where(x => x.Name == name)
                .Where(x => x.AvailableDates.Any(y => y.From <= date && y.To >= date))
                .Any();
        }
    }
}
