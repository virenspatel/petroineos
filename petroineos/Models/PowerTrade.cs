using System;

namespace Petroineos.Models
{
    public class PowerTrade
    {
        public DateTime Date { get; set; }
        public PowerPeriod[] Periods { get; set; }
        public PowerTrade() { }
        public PowerTrade(DateTime date, int numberOfPeriods)
        {
            this.Date = date;
            this.Periods = new PowerPeriod[numberOfPeriods];
        }
    }
}
