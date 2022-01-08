using CardsAndMonsters.Data.Factories;
using CardsAndMonsters.Features.Battle;
using CardsAndMonsters.Features.Card;
using CardsAndMonsters.Features.Game;
using CardsAndMonsters.Features.GameOver;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Features.Opponent;
using CardsAndMonsters.Features.Position;
using CardsAndMonsters.Features.Storage;
using CardsAndMonsters.Features.Turn;
using CardsAndMonsters.Features.TurnPhase;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CardsAndMonsters.Features.Tests.Game
{
    public class GameServiceTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IDuelistFactory> _mockDuelistFactory;
        private readonly Mock<IBattleService> _mockBattleService;
        private readonly Mock<ITurnService> _mockTurnService;
        private readonly Mock<IPhaseService> _mockPhaseService;
        private readonly Mock<IPositionService> _mockPositionService;
        private readonly Mock<IFakeOpponentService> _mockFakeOpponentService;
        private readonly Mock<IGameOverService> _mockGameOverService;
        private readonly Mock<IDuelLogService> _mockDuelLogService;
        private readonly Mock<ICardService> _mockCardService;
        private readonly Mock<IBoardManagementService> _mockBoardManagementService;

        public GameServiceTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockDuelistFactory = _mockRepository.Create<IDuelistFactory>();
            _mockBattleService = _mockRepository.Create<IBattleService>();
            _mockTurnService = _mockRepository.Create<ITurnService>();
            _mockPhaseService = _mockRepository.Create<IPhaseService>();
            _mockPositionService = _mockRepository.Create<IPositionService>();
            _mockFakeOpponentService = _mockRepository.Create<IFakeOpponentService>();
            _mockGameOverService = _mockRepository.Create<IGameOverService>();
            _mockDuelLogService = _mockRepository.Create<IDuelLogService>();
            _mockCardService = _mockRepository.Create<ICardService>();
            _mockBoardManagementService = _mockRepository.Create<IBoardManagementService>();
        }

        private GameService CreateService()
        {
            return new GameService(_mockDuelistFactory.Object, _mockBattleService.Object,
                _mockTurnService.Object, _mockPhaseService.Object, _mockPositionService.Object,
                _mockFakeOpponentService.Object, _mockGameOverService.Object, _mockDuelLogService.Object,
                _mockCardService.Object, _mockBoardManagementService.Object);
        }

        [Fact]
        public async Task CheckForExistingGame_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();

            // Act
            var result = await service.CheckForExistingGame();

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task ResumeGame_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();

            // Act
            await service.ResumeGame();

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task NewGame_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();

            // Act
            await service.NewGame();

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task ClearGame_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();

            // Act
            await service.ClearGame();

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task EnterPhase_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            Phase phase = default;

            // Act
            await service.EnterPhase(phase);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void PlayCard_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            BaseCard card = null;

            // Act
            service.PlayCard(card);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void PlayMonster_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            Monster monster = null;

            // Act
            service.PlayMonster(monster);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task PlayMonster_StateUnderTest_ExpectedBehavior1()
        {
            // Arrange
            var service = CreateService();
            FieldPosition position = default;

            // Act
            await service.PlayMonster(position);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Attack_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            BattleInfo battleInfo = null;

            // Act
            await service.Attack(battleInfo);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task SwitchPosition_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            Monster monster = null;

            // Act
            await service.SwitchPosition(monster);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task EndTurn_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();

            // Act
            await service.EndTurn();

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Dispose_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();

            // Act
            service.Dispose();

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }
    }
}
