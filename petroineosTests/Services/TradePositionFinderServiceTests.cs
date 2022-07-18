using Moq;
using NUnit.Framework;
using Petroineos.Services;
using Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetroineosTests.Services
{
    public class TradePositionFinderServiceTests
    {
        private TradePositionFinderService _sut;
        private Mock<IPowerService> _powerServiceMocked;

        [SetUp]
        public void SetUp()
        {
            _powerServiceMocked = new Mock<IPowerService>();
            _sut = new TradePositionFinderService(_powerServiceMocked.Object);
        }

        [Test]
        public async Task Verify_GetTrades_Called()
        {
            _powerServiceMocked.Setup(x => x.GetTradesAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(new List<PowerTrade>());

            var result = await _sut.GetPowerTrade(DateTime.Now);

            _powerServiceMocked.Verify(x => x.GetTradesAsync(It.IsAny<DateTime>()), Times.Once);
        }
    }
}
