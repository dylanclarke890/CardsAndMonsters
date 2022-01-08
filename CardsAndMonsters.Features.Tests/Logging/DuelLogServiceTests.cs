using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Enums;
using CardsAndMonsters.Models.Logging;
using Moq;
using System.Collections.Generic;
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
        public void AddNewEventLog_ValidParameters_CompletesSuccessfully()
        {
            // Arrange
            var service = CreateService();
            Event eventType = Event.GameStarted;
            Duelist duelist = new("test");

            // Act
            service.AddNewEventLog(eventType, duelist);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void GetEventLogs_ReturnsExpectedList()
        {
            // Arrange
            var service = CreateService();

            Duelist duelist = new("test");
            service.AddNewEventLog(Event.GameStarted, duelist);
            service.AddNewEventLog(Event.TurnChange, duelist);

            // Act
            var result = service.GetEventLogs();

            // Assert
            List<EventLog> expected = new()
            {
                new() { Description = $"test ended turn", Event = Event.TurnChange },
                new() { Description = $"Game started with test going first", Event = Event.GameStarted },
            };

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].Event, result[i].Event);
                Assert.Equal(expected[i].Description, result[i].Description);
            }

            _mockRepository.VerifyAll();
        }
    }
}
