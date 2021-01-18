using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VimServer.Provider
{
    public partial class Provider
    {
        public string Name { get; set; }
        public double Score { get; set; }
        public string[] Specialties { get; set; }
        public AvailableDate[] AvailableDates { get; set; } = new AvailableDate[0];
    }

    public partial class AvailableDate
    {
        public long From { get; set; }
        public long To { get; set; }
    }
}
