using CardsAndMonsters.Features.Storage;
using CardsAndMonsters.Models;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CardsAndMonsters.Features.Tests.Storage
{
    public class BoardManagementServiceTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<ILocalStorageService<Board>> _mockLocalStorageService;

        public BoardManagementServiceTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockLocalStorageService = _mockRepository.Create<ILocalStorageService<Board>>();
        }

        private BoardManagementService CreateService()
        {
            return new BoardManagementService(_mockLocalStorageService.Object);
        }

        [Fact]
        public async Task Save_NonNullBoard_CompletesSuccessfully()
        {
            // Arrange

            Board board = new();
            _mockLocalStorageService.Setup(lss => lss.SetItem("boardStorage", board))
                .Returns(Task.CompletedTask);

            var service = CreateService();

            // Act
            await service.Save(board);

            // Assert
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Load_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            _mockLocalStorageService.Setup(lss => lss.GetItem("boardStorage"))
                .Returns(Task.FromResult(new Board()));

            var service = CreateService();

            // Act
            var result = await service.Load();

            // Assert

            Assert.NotNull(result);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            _mockLocalStorageService.Setup(lss => lss.DeleteItem("boardStorage"))
                .Returns(Task.CompletedTask);

            var service = CreateService();

            // Act
            await service.Delete();

            // Assert
            _mockRepository.VerifyAll();
        }
    }
}
