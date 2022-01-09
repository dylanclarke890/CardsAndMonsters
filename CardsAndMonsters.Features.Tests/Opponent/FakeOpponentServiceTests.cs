using CardsAndMonsters.Core.Exceptions;
using CardsAndMonsters.Features.Battle;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Features.Opponent;
using CardsAndMonsters.Features.Position;
using CardsAndMonsters.Features.RandomNumber;
using CardsAndMonsters.Features.Storage;
using CardsAndMonsters.Features.Turn;
using CardsAndMonsters.Features.TurnPhase;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Enums;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CardsAndMonsters.Features.Tests.Opponent
{
    public class FakeOpponentServiceTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IBattleService> _mockBattleService;
        private readonly Mock<IBoardManagementService> _mockBoardManagementService;
        private readonly Mock<IDuelLogService> _mockDuelLogService;
        private readonly Mock<INumberGenerator> _mockNumberGenerator;
        private readonly Mock<IPhaseService> _mockPhaseService;
        private readonly Mock<IPositionService> _mockPositionService;
        private readonly Mock<ITurnService> _mockTurnService;

        public FakeOpponentServiceTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockBattleService = _mockRepository.Create<IBattleService>();
            _mockBoardManagementService = _mockRepository.Create<IBoardManagementService>();
            _mockDuelLogService = _mockRepository.Create<IDuelLogService>();
            _mockNumberGenerator = _mockRepository.Create<INumberGenerator>();
            _mockPhaseService = _mockRepository.Create<IPhaseService>();
            _mockPositionService = _mockRepository.Create<IPositionService>();
            _mockTurnService = _mockRepository.Create<ITurnService>();
        }

        private FakeOpponentService CreateService()
        {
            return new FakeOpponentService(_mockBattleService.Object, _mockBoardManagementService.Object,
                _mockDuelLogService.Object, _mockNumberGenerator.Object, _mockPhaseService.Object,
                _mockPositionService.Object, _mockTurnService.Object);
        }

        [Fact]
        public async Task FakeMainPhase_NoPlayableActions_CompletesSuccessfully()
        {
            // Arrange
            Duelist player = new();
            Duelist opponent = new();
            Board board = new(player, opponent);

            _mockBoardManagementService.Setup(bms => bms.Save(board))
                .Returns(Task.CompletedTask);

            var service = CreateService();

            // Act
            await service.FakeMainPhase(board);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task FakeMainPhase_NullBoard_ThrowsGameArgumentException()
        {
            // Arrange
            var service = CreateService();
            Board board = null;

            // Act
            async Task act() => await service.FakeMainPhase(board);

            // Assert
            await Assert.ThrowsAsync<GameArgumentException<Board>>(act);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task FakeBattlePhase_NoPlayableActions_CompletesSuccessfully()
        {
            // Arrange
            Board board = new();

            _mockPhaseService.Setup(ps => ps.EnterPhase(Phase.Battle, board))
                .Returns(Task.CompletedTask);
            _mockBoardManagementService.Setup(bms => bms.Save(board))
                .Returns(Task.CompletedTask);

            var service = CreateService();

            // Act
            await service.FakeBattlePhase(board);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task FakeBattlePhase_NullBoard_ThrowsGameArgumentException()
        {
            // Arrange
            var service = CreateService();
            Board board = null;

            // Act
            async Task act() => await service.FakeBattlePhase(board);

            // Assert
            await Assert.ThrowsAsync<GameArgumentException<Board>>(act);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task FakeEndPhase_NoPlayableActions_CompletesSuccessfully()
        {
            // Arrange

            Board board = new();

            _mockTurnService.Setup(ts => ts.EndTurn(board))
                .Returns(Task.CompletedTask);
            _mockBoardManagementService.Setup(bms => bms.Save(board))
                .Returns(Task.CompletedTask);

            var service = CreateService();

            // Act
            await service.FakeEndPhase(board);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task FakeEndPhase_NullBoard_ThrowsGameArgumentException()
        {
            // Arrange
            var service = CreateService();
            Board board = null;

            // Act
            async Task act() => await service.FakeEndPhase(board);

            // Assert
            await Assert.ThrowsAsync<GameArgumentException<Board>>(act);

            _mockRepository.VerifyAll();
        }

        [Theory]
        [InlineData(Phase.Standby, 4)]
        [InlineData(Phase.Main, 3)]
        [InlineData(Phase.Battle, 2)]
        [InlineData(Phase.End, 0)]
        public async Task ResumePhase_FromDifferentPhases_CallsChangePhaseExpectedNumOfTimes(Phase phase, int expectedAmount)
        {
            // Arrange
            Board board = new() { CurrentTurn = new() { Phase =  phase} };

            _mockBoardManagementService.Setup(bms => bms.Save(board))
                .Returns(Task.CompletedTask);
            if (phase is not Phase.End)
            {
                _mockPhaseService.Setup(ps => ps.EnterPhase(It.IsAny<Phase>(), board))
                        .Returns((Phase ph, Board b) =>
                        {
                            b.CurrentTurn.Phase = ph;
                            return Task.CompletedTask;
                        });
            }
            _mockTurnService.Setup(ts => ts.EndTurn(board))
                    .Returns(Task.CompletedTask);

            var service = CreateService();

            // Act
            await service.ResumePhase(board);

            // Assert
            _mockPhaseService.Verify(ps => ps.EnterPhase(It.IsAny<Phase>(), board), Times.Exactly(expectedAmount));
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task ResumePhase_NullBoard_ThrowsGameArgumentException()
        {
            // Arrange
            var service = CreateService();
            Board board = null;

            // Act
            async Task act() => await service.ResumePhase(board);

            // Assert
            await Assert.ThrowsAsync<GameArgumentException<Board>>(act);

            _mockRepository.VerifyAll();
        }
    }
}
