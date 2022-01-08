﻿using CardsAndMonsters.Features.Battle;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Features.Opponent;
using CardsAndMonsters.Features.Position;
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
    }
}
