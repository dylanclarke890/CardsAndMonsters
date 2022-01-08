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
        public async Task Save_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            Board board = null;

            // Act
            await service.Save(board);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Load_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();

            // Act
            var result = await service.Load();

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();

            // Act
            await service.Delete();

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }
    }
}
