using System.Collections.Generic;

namespace VimServer
{
    public interface IAppointmentsService
    {
        IEnumerable<string> GetAvailableProviders(string specialty, long date, double minScore);
        bool TryBookAppointment(string name, long date);
    }
}