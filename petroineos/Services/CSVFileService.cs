using CsvHelper;
using log4net;
using Petroineos.Interfaces;
using Petroineos.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Petroineos.Services
{
    public class CSVFileService : ICSVFileService
    {
        private readonly ILog _log;
        private readonly IConfigReaderService _configReader;
        private readonly IIOService _IOService;
        public CSVFileService(ILog log, IConfigReaderService configReader, IIOService iOService)
        {
            _log = log;
            _configReader = configReader;
            _IOService = iOService;
        }
        public string GetFileNameWithPath()
        {
            var todayFileName = $"PowerPosition_{DateTime.Now.AddHours(1):yyyyMMdd}";
            var files = _IOService.DirectoryGetFiles(_configReader.CsvFilePath, todayFileName + "*.csv");

            if (files.Any())
            {
                return files[0];
            }
            return $"{_configReader.CsvFilePath}PowerPosition_{DateTime.Now:yyyyMMdd_HHmm}.csv";
        }

        public bool Write(string fileWithPath, IEnumerable<Models.PowerTrade> powerTrades)
        {
            List<PowerReport> powerReports = GetPowerReports(powerTrades);

            if (_IOService.FileExists(fileWithPath))
            {
                _IOService.DeleteFile(fileWithPath);
            }

            using (var writer = new StreamWriter(fileWithPath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(powerReports);
                _log.Info("CSV file has been created.");
            }

            return true;
        }

        public List<PowerReport> GetPowerReports(IEnumerable<PowerTrade> powerTrades)
        {
            var powerReports = new List<PowerReport>();

            var periods = powerTrades.SelectMany(x => x.Periods).GroupBy(x => x.Period);

            powerReports = periods.Select(x => new PowerReport
            {
                LocalTime = GetLocalTime(x.Key),
                Volumn = (int)x.ToList().Sum(s => s.Volume)
            }).ToList();
            return powerReports;
        }

        private string GetLocalTime(int period)
        {
            //period 1, - 2 == 23:00 previous day
            return DateTime.Now.Date.AddHours(period - 2).ToString("HH:mm");
        }
    }
}