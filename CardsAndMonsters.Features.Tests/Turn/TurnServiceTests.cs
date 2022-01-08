using CardsAndMonsters.Features.GameOver;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Features.Turn;
using CardsAndMonsters.Features.TurnPhase;
using CardsAndMonsters.Models;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CardsAndMonsters.Features.Tests.Turn
{
    public class TurnServiceTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IPhaseService> _mockPhaseService;
        private readonly Mock<IGameOverService> _mockGameOverService;
        private readonly Mock<IDuelLogService> _mockDuelLogService;

        public TurnServiceTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockPhaseService = _mockRepository.Create<IPhaseService>();
            _mockGameOverService = _mockRepository.Create<IGameOverService>();
            _mockDuelLogService = _mockRepository.Create<IDuelLogService>();
        }

        private TurnService CreateService()
        {
            return new TurnService(_mockPhaseService.Object, _mockGameOverService.Object,
                _mockDuelLogService.Object);
        }

        [Fact]
        public async Task StartTurn_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            Duelist duelist = null;
            bool drawCard = false;
            Board board = null;

            // Act
            await service.StartTurn(duelist, drawCard, board);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task ResumeTurn_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            Board board = null;

            // Act
            await service.ResumeTurn(board);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task EndTurn_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            Board board = null;

            // Act
            await service.EndTurn(board);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }
    }
}
