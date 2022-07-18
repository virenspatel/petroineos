using log4net;
using Moq;
using NUnit.Framework;
using Petroineos.Interfaces;
using Petroineos.Models;
using Petroineos.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetroineosTests.Services
{
    public class ReportCreatorServiceTests
    {
        private ReportCreatorService _sut;
        private Mock<ICSVFileService> _CSVFileMocked;
        private Mock<IConfigReaderService> _configReaderServiceMocked;
        private Mock<ITradePositionFinderService> _tradePositionFinderServiceMocked;
        private Mock<ILog> _logMocked;
        private Mock<IIOService> _directoryServiceMocked;

        [SetUp]
        public void SetUp()
        {
            _CSVFileMocked = new Mock<ICSVFileService>();
            _configReaderServiceMocked = new Mock<IConfigReaderService>();
            _tradePositionFinderServiceMocked = new Mock<ITradePositionFinderService>();
            _logMocked = new Mock<ILog>();
            _directoryServiceMocked = new Mock<IIOService>();

            _sut = new ReportCreatorService(_tradePositionFinderServiceMocked.Object,
                _CSVFileMocked.Object,
                _configReaderServiceMocked.Object,
                _logMocked.Object,
                _directoryServiceMocked.Object);
        }

        [Test]
        public async Task Verify_GetFileNameWithPath_Called_When_Trade_Exists_And_FilePath_Are_Valid()
        {
            var csvFileName = "csvFileName";
            var csvFileNameWithPath = "csvFileNameWithPath";
            var powerTrades = new List<PowerTrade>()
            {
                new PowerTrade
                {
                    Date = DateTime.Now
                }
            };


            _configReaderServiceMocked.Setup(x => x.CsvFilePath)
                .Returns(csvFileName);
            _tradePositionFinderServiceMocked.Setup(x => x.GetPowerTrade(It.IsAny<DateTime>()))
                .ReturnsAsync(powerTrades);
            _directoryServiceMocked.Setup(x => x.DirectoryExists(csvFileName)).Returns(true);
            _CSVFileMocked.Setup(x => x.GetFileNameWithPath()).Returns(csvFileNameWithPath);

            await _sut.Process();

            _tradePositionFinderServiceMocked.Verify(x => x.GetPowerTrade(It.IsAny<DateTime>()), Times.Once);
            _CSVFileMocked.Verify(x => x.Write(csvFileNameWithPath, powerTrades), Times.Once);
        }

        [Test]
        public async Task Verify_Logs_Being_Generated_When_Trade_Exists_And_FilePath_Are_Invalid()
        {
            var csvFileName = "csvFileName";
            var logMessage = "CSVFilePath doesn't exists. Please change path in config file.";
            var csvFileNameWithPath = "csvFileNameWithPath";
            var powerTrades = new List<PowerTrade>()
            {
                new PowerTrade
                {
                    Date = DateTime.Now
                }
            };


            _configReaderServiceMocked.Setup(x => x.CsvFilePath)
                .Returns(csvFileName);
            _tradePositionFinderServiceMocked.Setup(x => x.GetPowerTrade(It.IsAny<DateTime>()))
                .ReturnsAsync(powerTrades);
            _directoryServiceMocked.Setup(x => x.DirectoryExists(csvFileName)).Returns(false);
            _CSVFileMocked.Setup(x => x.GetFileNameWithPath()).Returns(csvFileNameWithPath);
            _logMocked.Setup(x => x.Debug(logMessage));

            await _sut.Process();

            _logMocked.Verify(x => x.Debug(logMessage), Times.Once);
            _tradePositionFinderServiceMocked.Verify(x => x.GetPowerTrade(It.IsAny<DateTime>()), Times.Once);
            _CSVFileMocked.Verify(x => x.Write(csvFileNameWithPath, powerTrades), Times.Never);
        }
    }
}
