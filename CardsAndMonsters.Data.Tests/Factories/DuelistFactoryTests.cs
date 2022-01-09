using CardsAndMonsters.Data.Factories;
using Moq;
using Xunit;

namespace CardsAndMonsters.Data.Tests.Factories
{
    public class DuelistFactoryTests
    {
        private readonly MockRepository _mockRepository;

        public DuelistFactoryTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
        }

        private DuelistFactory CreateFactory()
        {
            return new DuelistFactory();
        }

        [Fact]
        public void GetNewPlayer_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var factory = CreateFactory();

            // Act
            var result = factory.GetNewPlayer();

            // Assert
            Assert.Equal("Player", result.Name);
            Assert.NotEmpty(result.Deck);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void GetNewOpponent_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var factory = CreateFactory();

            // Act
            var result = factory.GetNewOpponent();

            // Assert
            Assert.Equal("Opponent", result.Name);
            Assert.NotEmpty(result.Deck);

            _mockRepository.VerifyAll();
        }
    }
}
