using Petroineos.Models;
using System.Collections.Generic;

namespace Petroineos.Interfaces
{
    public interface ICSVFileService
    {
        string GetFileNameWithPath();
        bool Write(string path, IEnumerable<Models.PowerTrade> powerTrades);
        List<PowerReport> GetPowerReports(IEnumerable<PowerTrade> powerTrades);
    }
}
