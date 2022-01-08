using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Features.Position;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using Moq;
using Xunit;

namespace CardsAndMonsters.Features.Tests.Position
{
    public class PositionServiceTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IDuelLogService> _mockDuelLogService;

        public PositionServiceTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockDuelLogService = _mockRepository.Create<IDuelLogService>();
        }

        private PositionService CreateService()
        {
            return new PositionService(_mockDuelLogService.Object);
        }

        [Fact]
        public void NewPosition_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            FieldPosition old = default;

            // Act
            var result = service.NewPosition(old);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void PositionSwitched_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            Monster monster = null;
            Board board = null;

            // Act
            service.PositionSwitched(monster, board);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }
    }
}
