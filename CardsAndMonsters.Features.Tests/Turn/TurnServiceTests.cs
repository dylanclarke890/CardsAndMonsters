using CardsAndMonsters.Core.Exceptions;
using CardsAndMonsters.Features.GameOver;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Features.Turn;
using CardsAndMonsters.Features.TurnPhase;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CardsAndMonsters.Features.Tests.Turn
{
    public class TurnServiceTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IDuelLogService> _mockDuelLogService;
        private readonly Mock<IGameOverService> _mockGameOverService;
        private readonly Mock<IPhaseService> _mockPhaseService;

        public TurnServiceTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockDuelLogService = _mockRepository.Create<IDuelLogService>();
            _mockGameOverService = _mockRepository.Create<IGameOverService>();
            _mockPhaseService = _mockRepository.Create<IPhaseService>();
        }

        private TurnService CreateService()
        {
            return new TurnService(_mockDuelLogService.Object, _mockGameOverService.Object,
                _mockPhaseService.Object);
        }

        [Fact]
        public async Task StartTurn_DrawCardFalse_CompletesSuccessfully()
        {
            // Arrange
            Board board = new();
            Duelist duelist = new("test");

            _mockPhaseService.Setup(ps => ps.EnterPhase(Phase.Standby, board)).Returns(Task.CompletedTask);
            _mockPhaseService.Setup(ps => ps.EnterPhase(Phase.Main, board)).Returns(Task.CompletedTask);

            var service = CreateService();
            bool drawCard = false;

            // Act
            await service.StartTurn(duelist, drawCard, board);

            // Assert
            Assert.Equal(0, board.CurrentTurn.CardsDrawn);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task StartTurn_DrawCardTrue_EndsGameIfDrawingFails()
        {
            // Arrange
            Board board = new();
            Duelist duelist = new("test");

            _mockPhaseService.Setup(ps => ps.EnterPhase(Phase.Standby, board))
                .Returns(Task.CompletedTask);
            _mockDuelLogService.Setup(ps => ps.AddNewEventLog(Event.DrawCard, duelist));
            _mockGameOverService.Setup(gos => gos.EndGame(duelist, LossReason.DeckOut))
                .Returns(Task.CompletedTask);

            var service = CreateService();
            bool drawCard = true;

            // Act
            await service.StartTurn(duelist, drawCard, board);

            // Assert
            Assert.Equal(1, board.CurrentTurn.CardsDrawn);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task StartTurn_DrawCardTrue_EntersMainPhaseIfDrawingSuccess()
        {
            // Arrange
            Board board = new();
            Duelist duelist = new("test");
            duelist.Deck.Add(new Monster(100, 100));

            _mockPhaseService.Setup(ps => ps.EnterPhase(Phase.Standby, board))
                .Returns(Task.CompletedTask);
            _mockPhaseService.Setup(ps => ps.EnterPhase(Phase.Main, board))
                .Returns(Task.CompletedTask);
            _mockDuelLogService.Setup(ps => ps.AddNewEventLog(Event.DrawCard, duelist));

            var service = CreateService();
            bool drawCard = true;

            // Act
            await service.StartTurn(duelist, drawCard, board);

            // Assert
            Assert.Equal(1, board.CurrentTurn.CardsDrawn);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task StartTurn_NullBoard_ThrowsGameArgumentException()
        {
            // Arrange
            var service = CreateService();
            Board board = null;
            Duelist duelist = new("test");
            bool drawCard = true;

            // Act
            async Task act() => await service.StartTurn(duelist, drawCard, board);

            // Assert
            await Assert.ThrowsAsync<GameArgumentException<Board>>(act);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task StartTurn_NullDuelist_ThrowsGameArgumentException()
        {
            // Arrange
            var service = CreateService();
            Board board = new();
            Duelist duelist = null;
            bool drawCard = true;

            // Act
            async Task act() => await service.StartTurn(duelist, drawCard, board);

            // Assert
            await Assert.ThrowsAsync<GameArgumentException<Board>>(act);

            _mockRepository.VerifyAll();
        }

        [Theory]
        [InlineData(Phase.Standby)]
        [InlineData(Phase.Main)]
        [InlineData(Phase.Battle)]
        [InlineData(Phase.End)]
        public async Task ResumeTurn_DifferentPhaseParameters_ResumesLastPhase(Phase phase)
        {
            // Arrange
            Board board = new() { CurrentTurn = new() { Phase = phase } };

            _mockPhaseService.Setup(ps => ps.EnterPhase(phase, board))
                .Returns(Task.CompletedTask);

            var service = CreateService();

            // Act
            await service.ResumeTurn(board);

            // Assert

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task ResumeTurn_NullBoard_ThrowsGameArgumentException()
        {
            // Arrange
            var service = CreateService();
            Board board = null;

            // Act
            async Task act() => await service.ResumeTurn(board);

            // Assert
            await Assert.ThrowsAsync<GameArgumentException<Board>>(act);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task EndTurn_ValidBoardState_IncrementsTurnCountCorrectly()
        {
            // Arrange
            Duelist player = new("test");
            Duelist opponent = new("testopponent");
            Board board = new(player, opponent) { CurrentTurn = new() { Duelist = player } };
            player.Deck.Add(new Monster(100, 100));
            opponent.Deck.Add(new Monster(100, 100));

            _mockPhaseService.Setup(ps => ps.EnterPhase(Phase.End, board))
                .Returns(Task.CompletedTask);
            _mockPhaseService.Setup(ps => ps.EnterPhase(Phase.Standby, board))
                .Returns(Task.CompletedTask);
            _mockPhaseService.Setup(ps => ps.EnterPhase(Phase.Main, board))
                .Returns(Task.CompletedTask);

            _mockDuelLogService.Setup(ps => ps.AddNewEventLog(Event.TurnChange, player));
            _mockDuelLogService.Setup(ps => ps.AddNewEventLog(Event.DrawCard, opponent));

            var service = CreateService();

            // Act
            await service.EndTurn(board);

            // Assert
            Assert.Equal(1, board.TurnCount);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task EndTurn_NullBoard_ThrowsGameArgumentException()
        {
            // Arrange
            var service = CreateService();
            Board board = null;

            // Act
            async Task act() => await service.EndTurn(board);

            // Assert
            await Assert.ThrowsAsync<GameArgumentException<Board>>(act);

            _mockRepository.VerifyAll();
        }
    }
}
