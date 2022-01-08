using CardsAndMonsters.Features.Battle;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Features.Opponent;
using CardsAndMonsters.Features.Position;
using CardsAndMonsters.Features.Storage;
using CardsAndMonsters.Features.Turn;
using CardsAndMonsters.Features.TurnPhase;
using CardsAndMonsters.Models;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CardsAndMonsters.Features.Tests.Opponent
{
    public class FakeOpponentServiceTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IBattleService> _mockBattleService;
        private readonly Mock<IPhaseService> _mockPhaseService;
        private readonly Mock<ITurnService> _mockTurnService;
        private readonly Mock<IPositionService> _mockPositionService;
        private readonly Mock<IDuelLogService> _mockDuelLogService;
        private readonly Mock<IBoardManagementService> _mockBoardManagementService;

        public FakeOpponentServiceTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockBattleService = _mockRepository.Create<IBattleService>();
            _mockPhaseService = _mockRepository.Create<IPhaseService>();
            _mockTurnService = _mockRepository.Create<ITurnService>();
            _mockPositionService = _mockRepository.Create<IPositionService>();
            _mockDuelLogService = _mockRepository.Create<IDuelLogService>();
            _mockBoardManagementService = _mockRepository.Create<IBoardManagementService>();
        }

        private FakeOpponentService CreateService()
        {
            return new FakeOpponentService(_mockBattleService.Object, _mockPhaseService.Object, 
                _mockTurnService.Object, _mockPositionService.Object, _mockDuelLogService.Object,
                _mockBoardManagementService.Object);
        }

        [Fact]
        public async Task FakeMainPhase_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            Board board = null;

            // Act
            await service.FakeMainPhase(board);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task FakeBattlePhase_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            Board board = null;

            // Act
            await service.FakeBattlePhase(board);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task FakeEndPhase_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            Board board = null;

            // Act
            await service.FakeEndPhase(board);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task ResumePhase_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            Board board = null;

            // Act
            await service.ResumePhase(board);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }
    }
}
