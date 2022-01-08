using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Features.TurnPhase;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Enums;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CardsAndMonsters.Features.Tests.TurnPhase
{
    public class PhaseServiceTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IDuelLogService> _mockDuelLogService;

        public PhaseServiceTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockDuelLogService = _mockRepository.Create<IDuelLogService>();
        }

        private PhaseService CreateService()
        {
            return new PhaseService(_mockDuelLogService.Object);
        }

        [Fact]
        public async Task EnterPhase_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            Phase phase = default;
            Board board = null;

            // Act
            await service.EnterPhase(phase, board);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }
    }
}
