using Petroineos.Interfaces;
using Petroineos.Mappers;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Petroineos.Services
{
    public class TradePositionFinderService : ITradePositionFinderService
    {
        private readonly IPowerService _powerService;

        public TradePositionFinderService(IPowerService powerService)
        {
            this._powerService = powerService;
        }

        public async Task<IEnumerable<Models.PowerTrade>> GetPowerTrade(DateTime dateTime)
        {
            var powerTrade = await this._powerService.GetTradesAsync(dateTime);
            return powerTrade.Select(x => PowerTraderMapper.Map(x));
        }
    }
}
