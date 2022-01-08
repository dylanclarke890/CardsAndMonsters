using CardsAndMonsters.Features.GameOver;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Features.Storage;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Enums;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CardsAndMonsters.Features.Tests.GameOver
{
    public class GameOverServiceTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IDuelLogService> _mockDuelLogService;
        private readonly Mock<IBoardManagementService> _mockBoardManagementService;

        public GameOverServiceTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockDuelLogService = _mockRepository.Create<IDuelLogService>();
            _mockBoardManagementService = _mockRepository.Create<IBoardManagementService>();
        }

        private GameOverService CreateService()
        {
            return new GameOverService(_mockDuelLogService.Object, _mockBoardManagementService.Object);
        }

        [Fact]
        public async Task CheckForGameOver_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            Board board = null;

            // Act
            await service.CheckForGameOver(board);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task EndGame_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            Duelist duelist = null;
            LossReason reason = default;

            // Act
            await service.EndGame(duelist, reason);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void ClearGameOverInfo_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();

            // Act
            service.ClearGameOverInfo();

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }
    }
}
