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
    public class CSVFileServiceTests
    {
        private CSVFileService _sut;
        private Mock<IConfigReaderService> _configReaderServiceMocked;
        private Mock<ILog> _logMocked;
        private Mock<IIOService> _IOServiceMocked;

        [SetUp]
        public void SetUp()
        {
            _configReaderServiceMocked = new Mock<IConfigReaderService>();
            _logMocked = new Mock<ILog>();
            _IOServiceMocked = new Mock<IIOService>();

            _sut = new CSVFileService(
                _logMocked.Object,
                _configReaderServiceMocked.Object,
                _IOServiceMocked.Object);
        }

        [Test]
        public void Should_Return_Same_File_Name_If_Exists_for_Date()
        {
            var fileExistedInFolder = "file1";
            _IOServiceMocked.Setup(x => x.DirectoryGetFiles(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new string[] { fileExistedInFolder });

            var result = _sut.GetFileNameWithPath();

            Assert.AreEqual(fileExistedInFolder, result);
        }

        [Test]
        public void Should_Return_New_File_Name_If_Dont_Exists()
        {
            var csvFilePath = "CsvFilePath";
            _configReaderServiceMocked.Setup(x => x.CsvFilePath).Returns(csvFilePath);
            _IOServiceMocked.Setup(x => x.DirectoryGetFiles(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new string[0] );

            var result = _sut.GetFileNameWithPath();

            Assert.AreEqual($"{csvFilePath}PowerPosition_{DateTime.Now:yyyyMMdd_HHmm}.csv", result);
        }

        [Test]
        public void Should_Delete_File_If_Exists()
        {
            var csvFileWithPath = "csvFileWithPath";
            var powerTrades = new List<PowerTrade>()
            {
                new PowerTrade
                {
                    Date = DateTime.Now,
                    Periods = new PowerPeriod []
                    {
                        new PowerPeriod
                        {
                            Period = 1,
                            Volume = 256.36
                        },
                        new PowerPeriod
                        {
                            Period = 2,
                            Volume = 258.36
                        },
                    }
                }
            };

            _IOServiceMocked.Setup(x => x.FileExists(csvFileWithPath)).Returns(true);

            var result = _sut.Write(csvFileWithPath, powerTrades);

            _IOServiceMocked.Verify(x => x.DeleteFile(csvFileWithPath), Times.Once);
        }

        [Test]
        public void Should_Aggregate_Volumn_and_Create_Power_Report()
        {
            var powerTrades = new List<PowerTrade>()
            {
                new PowerTrade
                {
                    Date = DateTime.Now.Date,
                    Periods = new PowerPeriod []
                    {
                        new PowerPeriod
                        {
                            Period = 1,
                            Volume = 256.36
                        },
                        new PowerPeriod
                        {
                            Period = 2,
                            Volume = 695
                        },
                    }
                },
                 new PowerTrade
                {
                    Date = DateTime.Now.Date,
                    Periods = new PowerPeriod []
                    {
                        new PowerPeriod
                        {
                            Period = 1,
                            Volume = 554.32
                        },
                        new PowerPeriod
                        {
                            Period = 2,
                            Volume = 586.32
                        },
                    }
                }
            };

            var expected = new List<PowerReport>
            {
                new PowerReport
                {
                    LocalTime = "23:00",
                    Volumn = 810
                },
                new PowerReport
                {
                    LocalTime = "00:00",
                    Volumn = 1281
                }
            };

            var result = _sut.GetPowerReports( powerTrades);

            Assert.AreEqual(expected[0].LocalTime, result[0].LocalTime);
            Assert.AreEqual(expected[0].Volumn, result[0].Volumn);
            Assert.AreEqual(expected[1].LocalTime, result[1].LocalTime);
            Assert.AreEqual(expected[1].Volumn, result[1].Volumn);
        }
    }
}
