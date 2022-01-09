using CardsAndMonsters.Features.GameOver;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Features.Storage;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Enums;
using CardsAndMonsters.Models.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CardsAndMonsters.Features.Tests.GameOver
{
    public class GameOverServiceTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IBoardManagementService> _mockBoardManagementService;
        private readonly Mock<IDuelLogService> _mockDuelLogService;

        public GameOverServiceTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockBoardManagementService = _mockRepository.Create<IBoardManagementService>();
            _mockDuelLogService = _mockRepository.Create<IDuelLogService>();
        }

        private GameOverService CreateService()
        {
            return new GameOverService(_mockBoardManagementService.Object, _mockDuelLogService.Object);
        }

        [Fact]
        public async Task CheckForGameOver_NeitherDuelistOutOfHp_NoActionTaken()
        {
            // Arrange
            var service = CreateService();

            Duelist player = new() { HP = 100 };
            Duelist opponent = new() { HP = 100 };
            Board board = new(player, opponent);

            // Act
            await service.CheckForGameOver(board);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task CheckForGameOver_DuelistOutOfHp_EndsGame()
        {
            // Arrange
            Duelist player = new() { HP = 0 };
            Duelist opponent = new() { HP = 100 };

            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.GameEnded, opponent));
            _mockDuelLogService.Setup(dls => dls.GetEventLogs())
                .Returns(new List<EventLog>());
            _mockBoardManagementService.Setup(bms => bms.Delete())
                .Returns(Task.CompletedTask);


            var service = CreateService();

            Board board = new(player, opponent);

            // Act
            await service.CheckForGameOver(board);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task EndGame_SetsGameOverToTrue()
        {
            // Arrange
            _mockDuelLogService.Setup(dls => dls.GetEventLogs())
                .Returns(new List<EventLog>());
            _mockBoardManagementService.Setup(bms => bms.Delete())
                .Returns(Task.CompletedTask);

            var service = CreateService();
            Duelist duelist = new();
            LossReason reason = LossReason.DeckOut;

            // Act
            await service.EndGame(duelist, reason);

            // Assert
            Assert.True(service.GameOver);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void ClearGameOverInfo_SetsGameOverToFalse()
        {
            // Arrange
            var service = CreateService();
            service.GameOver = true;

            // Act
            service.ClearGameOverInfo();

            // Assert
            Assert.False(service.GameOver);

            _mockRepository.VerifyAll();
        }
    }
}
