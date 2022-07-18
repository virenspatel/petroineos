using log4net;
using Petroineos.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Petroineos.Services
{
    public class ReportCreatorService : IReportCreatorService
    {
        private readonly ITradePositionFinderService _tradePositionFinder;
        private readonly ICSVFileService _CSVFileName;
        private readonly IConfigReaderService _configReader;
        private readonly ILog _log;
        private readonly IIOService _directoryService;
        public ReportCreatorService(ITradePositionFinderService tradePositionFinder, 
            ICSVFileService CSVFileName, IConfigReaderService configReader, ILog log,
            IIOService directoryService)
        {
            _tradePositionFinder = tradePositionFinder;
            _CSVFileName = CSVFileName;
            _configReader = configReader;
            _log = log;
            _directoryService = directoryService;
        }
        public async Task Process()
        {
            _log.Info("ReportCreator Process has been called.");
            var csvFilePath = _configReader.CsvFilePath;
            var powerTrade = await _tradePositionFinder.GetPowerTrade(DateTime.Now.Date);

            if (powerTrade.Any())
            {
                if (!_directoryService.DirectoryExists(csvFilePath))
                {
                    _log.Debug("CSVFilePath doesn't exists. Please change path in config file.");
                }
                else
                {
                    var csvFileNameWithPath = _CSVFileName.GetFileNameWithPath();
                    _CSVFileName.Write(csvFileNameWithPath, powerTrade);
                }
            }
            return;
        }
    }
}
