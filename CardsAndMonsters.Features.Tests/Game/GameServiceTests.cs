using CardsAndMonsters.Data.Factories;
using CardsAndMonsters.Features.Battle;
using CardsAndMonsters.Features.Card;
using CardsAndMonsters.Features.Game;
using CardsAndMonsters.Features.GameOver;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Features.Opponent;
using CardsAndMonsters.Features.Position;
using CardsAndMonsters.Features.RandomNumber;
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

        private readonly Mock<IBattleService> _mockBattleService;
        private readonly Mock<IBoardManagementService> _mockBoardManagementService;
        private readonly Mock<ICardService> _mockCardService;
        private readonly Mock<IDuelistFactory> _mockDuelistFactory;
        private readonly Mock<IDuelLogService> _mockDuelLogService;
        private readonly Mock<IFakeOpponentService> _mockFakeOpponentService;
        private readonly Mock<IGameOverService> _mockGameOverService;
        private readonly Mock<INumberGenerator> _mockNumberGenerator;
        private readonly Mock<IPositionService> _mockPositionService;
        private readonly Mock<IPhaseService> _mockPhaseService;
        private readonly Mock<ITurnService> _mockTurnService;

        public GameServiceTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockBattleService = _mockRepository.Create<IBattleService>();
            _mockBoardManagementService = _mockRepository.Create<IBoardManagementService>();
            _mockCardService = _mockRepository.Create<ICardService>();
            _mockDuelistFactory = _mockRepository.Create<IDuelistFactory>();
            _mockDuelLogService = _mockRepository.Create<IDuelLogService>();
            _mockFakeOpponentService = _mockRepository.Create<IFakeOpponentService>();
            _mockGameOverService = _mockRepository.Create<IGameOverService>();
            _mockNumberGenerator= _mockRepository.Create<INumberGenerator>();
            _mockPhaseService = _mockRepository.Create<IPhaseService>();
            _mockPositionService = _mockRepository.Create<IPositionService>();
            _mockTurnService = _mockRepository.Create<ITurnService>();

            _mockPhaseService.SetupAllProperties();
        }

        private GameService CreateService()
        {
            return new GameService(_mockBattleService.Object, _mockBoardManagementService.Object,
                _mockCardService.Object, _mockDuelistFactory.Object, _mockDuelLogService.Object,
                _mockFakeOpponentService.Object, _mockGameOverService.Object, _mockNumberGenerator.Object,
                _mockPhaseService.Object, _mockPositionService.Object, _mockTurnService.Object);
        }

        [Fact]
        public async Task CheckForExistingGame_LoadReturnsObject_ReturnsTrue()
        {
            // Arrange
            _mockBoardManagementService.Setup(bms => bms.Load())
                .Returns(Task.FromResult(new Board()));

            var service = CreateService();

            // Act
            var result = await service.CheckForExistingGame();

            // Assert
            Assert.True(result);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task CheckForExistingGame_LoadReturnsNull_ReturnsFalse()
        {
            // Arrange

            _mockBoardManagementService.Setup(bms => bms.Load())
                .Returns(Task.FromResult<Board>(default));

            var service = CreateService();

            // Act
            var result = await service.CheckForExistingGame();

            // Assert
            Assert.False(result);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task ResumeGame_PlayerTurnZeroBeforeDrawing_OnlyDrawsInitialCards()
        {
            // Arrange
            Duelist player = new();
            Duelist opponent = new();
            Board board = new(player, opponent);
            board.CurrentTurn = new(player);

            _mockBoardManagementService.Setup(bms => bms.Load())
                .Returns(Task.FromResult(board));
            _mockTurnService.Setup(ts => ts.ResumeTurn(board))
                .Returns(Task.CompletedTask);
            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.DrawCard, player));
            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.DrawCard, opponent));
            _mockBoardManagementService.Setup(bms => bms.Save(board))
                .Returns(Task.CompletedTask);

            var service = CreateService();

            // Act
            await service.ResumeGame();

            // Assert

            _mockDuelLogService.Verify(dls => dls.AddNewEventLog(Event.DrawCard, player), Times.Exactly(5));
            _mockDuelLogService.Verify(dls => dls.AddNewEventLog(Event.DrawCard, opponent), Times.Exactly(5));

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task ResumeGame_OpponentTurnZeroBeforeDrawing_DrawsInitialCardsAndFakesTurn()
        {
            // Arrange
            Duelist player = new();
            Duelist opponent = new();
            Board board = new(player, opponent);
            board.CurrentTurn = new(opponent);

            _mockBoardManagementService.Setup(bms => bms.Load())
                .Returns(Task.FromResult(board));
            _mockTurnService.Setup(ts => ts.ResumeTurn(board))
                .Returns(Task.CompletedTask);
            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.DrawCard, player));
            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.DrawCard, opponent));
            _mockBoardManagementService.Setup(bms => bms.Save(board))
                .Returns(Task.CompletedTask);
            _mockFakeOpponentService.Setup(fop => fop.ResumePhase(board))
                .Returns(Task.CompletedTask);

            var service = CreateService();

            // Act
            await service.ResumeGame();

            // Assert

            _mockDuelLogService.Verify(dls => dls.AddNewEventLog(Event.DrawCard, player), Times.Exactly(5));
            _mockDuelLogService.Verify(dls => dls.AddNewEventLog(Event.DrawCard, opponent), Times.Exactly(5));
            _mockFakeOpponentService.Verify(fop => fop.ResumePhase(board), Times.Once());

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task NewGame_PlayerStarts_DrawsCardsAndEntersMainPhase()
        {
            // Arrange
            Duelist player = new();
            Duelist opponent = new();

            _mockDuelistFactory.Setup(df => df.GetNewPlayer())
                .Returns(player);
            _mockDuelistFactory.Setup(df => df.GetNewOpponent())
                .Returns(opponent);
            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.GameStarted, It.IsAny<Duelist>()));
            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.DrawCard, player));
            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.DrawCard, opponent));
            _mockBoardManagementService.Setup(bms => bms.Save(It.IsAny<Board>()))
                .Returns(Task.CompletedTask);
            _mockTurnService.Setup(ts => ts.StartTurn(It.IsAny<Duelist>(), false, It.IsAny<Board>()))
                .Returns(Task.CompletedTask);
            _mockNumberGenerator.Setup(ng => ng.GetRandomNumber(2))
                .Returns(0);

            var service = CreateService();

            // Act
            await service.NewGame();

            // Assert

            _mockDuelLogService.Verify(dls => dls.AddNewEventLog(Event.DrawCard, player), Times.Exactly(5));
            _mockDuelLogService.Verify(dls => dls.AddNewEventLog(Event.DrawCard, opponent), Times.Exactly(5));

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task NewGame_OpponentStarts_DrawsCardsAndFakesOpponentTurn()
        {
            // Arrange
            Duelist player = new();
            Duelist opponent = new();

            _mockDuelistFactory.Setup(df => df.GetNewPlayer())
                .Returns(player);
            _mockDuelistFactory.Setup(df => df.GetNewOpponent())
                .Returns(opponent);
            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.GameStarted, It.IsAny<Duelist>()));
            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.DrawCard, player));
            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.DrawCard, opponent));
            _mockBoardManagementService.Setup(bms => bms.Save(It.IsAny<Board>()))
                .Returns(Task.CompletedTask);
            _mockTurnService.Setup(ts => ts.StartTurn(It.IsAny<Duelist>(), false, It.IsAny<Board>()))
                .Returns(Task.CompletedTask);
            _mockNumberGenerator.Setup(ng => ng.GetRandomNumber(2))
                .Returns(1);
            _mockFakeOpponentService.Setup(fop => fop.FakeMainPhase(It.IsAny<Board>()))
                .Returns(Task.CompletedTask);
            _mockFakeOpponentService.Setup(fop => fop.FakeBattlePhase(It.IsAny<Board>()))
                .Returns(Task.CompletedTask);
            _mockFakeOpponentService.Setup(fop => fop.FakeEndPhase(It.IsAny<Board>()))
                .Returns(Task.CompletedTask);

            var service = CreateService();

            // Act
            await service.NewGame();

            // Assert

            _mockDuelLogService.Verify(dls => dls.AddNewEventLog(Event.DrawCard, player), Times.Exactly(5));
            _mockDuelLogService.Verify(dls => dls.AddNewEventLog(Event.DrawCard, opponent), Times.Exactly(5));

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task ClearGame_ClearsSuccessfully()
        {
            // Arrange

            _mockGameOverService.Setup(gos => gos.ClearGameOverInfo());
            _mockBoardManagementService.Setup(bms => bms.Delete())
                .Returns(Task.CompletedTask);

            var service = CreateService();

            // Act
            await service.ClearGame();

            // Assert
            _mockRepository.VerifyAll();
        }

        [Theory]
        [InlineData(Phase.Standby)]
        [InlineData(Phase.Main)]
        [InlineData(Phase.Battle)]
        [InlineData(Phase.End)]
        public async Task EnterPhase_WithPhaseParameter_CompletesSuccessfully(Phase phase)
        {
            // Arrange

            _mockPhaseService.Setup(ps => ps.EnterPhase(phase, null))
                .Returns(Task.CompletedTask);
            _mockBoardManagementService.Setup(bms => bms.Save(null))
                .Returns(Task.CompletedTask);

            var service = CreateService();

            // Act
            await service.EnterPhase(phase);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void PlayCard_CompletesSuccessfully()
        {
            // Arrange
            BaseCard card = new Monster(100, 100);

            _mockCardService.Setup(cs => cs.PlayCard(card, null));

            var service = CreateService();

            // Act
            service.PlayCard(card);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void PlayMonster_ValidMonster_CompletesSuccessfully()
        {
            // Arrange
            Monster monster = new(100, 100);

            _mockCardService.Setup(cs => cs.PlayMonster(monster));

            var service = CreateService();

            // Act
            service.PlayMonster(monster);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task PlayMonster_ValidPosition_CompletesSuccessfully()
        {
            // Arrange
            FieldPosition position = FieldPosition.HorizontalDown;

            _mockCardService.Setup(cs => cs.PlayMonster(position, null));
            _mockBoardManagementService.Setup(bms => bms.Save(null))
                .Returns(Task.CompletedTask);

            var service = CreateService();

            // Act
            await service.PlayMonster(position);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Attack_CompletesSuccessfully()
        {
            // Arrange
            BattleInfo battleInfo = new();

            _mockBattleService.Setup(bs => bs.Attack(battleInfo));
            _mockBoardManagementService.Setup(bms => bms.Save(null))
                .Returns(Task.CompletedTask);
            _mockGameOverService.Setup(gos => gos.CheckForGameOver(null))
                .Returns(Task.CompletedTask);

            var service = CreateService();

            // Act
            await service.Attack(battleInfo);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task SwitchPosition_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Monster monster = new(100, 100) { FieldPosition = FieldPosition.HorizontalDown };

            _mockPositionService.Setup(ps => ps.NewPosition(monster.FieldPosition))
                .Returns(FieldPosition.VerticalUp);
            _mockPositionService.Setup(ps => ps.PositionSwitched(monster, null));
            _mockBoardManagementService.Setup(bms => bms.Save(null))
                .Returns(Task.CompletedTask);
            
            var service = CreateService();

            // Act
            await service.SwitchPosition(monster);

            // Assert

            Assert.Equal(FieldPosition.VerticalUp, monster.FieldPosition);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task EndTurn_PlayerEndsTurn_FakesOpponentsTurn()
        {
            // Arrange

            Duelist player = new();
            Duelist opponent = new();
            Board board = new(player, opponent);
            board.CurrentTurn = new(player);

            _mockTurnService.Setup(ts => ts.EndTurn(board))
                .Returns(() => 
                {
                    board.CurrentTurn = new(opponent);
                    return Task.CompletedTask;
                });
            _mockFakeOpponentService.Setup(fop => fop.FakeMainPhase(board))
                .Returns(Task.CompletedTask);
            _mockFakeOpponentService.Setup(fop => fop.FakeBattlePhase(board))
                .Returns(Task.CompletedTask);
            _mockFakeOpponentService.Setup(fop => fop.FakeEndPhase(board))
                .Returns(Task.CompletedTask);

            var service = CreateService();
            service.Board = board;

            // Act
            await service.EndTurn();

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task EndTurn_OpponentEndsTurn_OnlyEndsTurn()
        {
            // Arrange

            Duelist player = new();
            Duelist opponent = new();
            Board board = new(player, opponent);
            board.CurrentTurn = new(opponent);

            _mockTurnService.Setup(ts => ts.EndTurn(board))
                .Returns(() =>
                {
                    board.CurrentTurn = new(player);
                    return Task.CompletedTask;
                });


            var service = CreateService();
            service.Board = board;

            // Act
            await service.EndTurn();

            // Assert
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
            _mockRepository.VerifyAll();
        }
    }
}
