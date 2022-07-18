using Petroineos.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Petroineos.Interfaces
{
    public interface ITradePositionFinderService
    {
        Task<IEnumerable<PowerTrade>> GetPowerTrade(DateTime dateTime);
    }
}
