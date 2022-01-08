using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Enums;
using Moq;
using Xunit;

namespace CardsAndMonsters.Features.Tests.Logging
{
    public class DuelLogServiceTests
    {
        private readonly MockRepository _mockRepository;

        public DuelLogServiceTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
        }

        private static DuelLogService CreateService()
        {
            return new DuelLogService();
        }

        [Fact]
        public void AddNewEventLog_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            Event eventType = default;
            Duelist duelist = null;

            // Act
            service.AddNewEventLog(eventType, duelist);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void GetEventLogs_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();

            // Act
            var result = service.GetEventLogs();

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }
    }
}
