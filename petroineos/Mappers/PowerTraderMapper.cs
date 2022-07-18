using Services;
using System.Linq;

namespace Petroineos.Mappers
{
    public static class PowerTraderMapper
    {
        public static Models.PowerTrade Map(this PowerTrade powerTrade)
        {
            return new Models.PowerTrade
            {
                Date = powerTrade.Date,
                Periods = powerTrade.Periods.Select
                    (x => new Models.PowerPeriod
                    {
                        Period = x.Period,
                        Volume = x.Volume
                    }).ToArray()
            };
        }
    }
}
