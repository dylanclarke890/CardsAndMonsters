using CardsAndMonsters.Features.Storage;
using CardsAndMonsters.Models;
using Microsoft.JSInterop;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CardsAndMonsters.Features.Tests.Storage
{
    public class LocalStorageServiceTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IJSRuntime> _mockJSRuntime;

        public LocalStorageServiceTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockJSRuntime = _mockRepository.Create<IJSRuntime>();
        }

        private LocalStorageService<Board> CreateService()
        {
            return new LocalStorageService<Board>(_mockJSRuntime.Object);
        }

        [Fact]
        public async Task SetItem_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            string key = null;
            Board item = null;

            // Act
            await service.SetItem(key, item);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetItem_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            string key = null;

            // Act
            var result = await service.GetItem(key);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task DeleteItem_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            string key = null;

            // Act
            await service.DeleteItem(key);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }
    }
}
