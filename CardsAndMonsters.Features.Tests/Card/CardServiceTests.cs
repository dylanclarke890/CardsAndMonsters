using CardsAndMonsters.Features.Card;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using Moq;
using Xunit;

namespace CardsAndMonsters.Features.Tests.Card
{
    public class CardServiceTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IDuelLogService> _mockDuelLogService;

        public CardServiceTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockDuelLogService = _mockRepository.Create<IDuelLogService>();
        }

        private CardService CreateService()
        {
            return new CardService(_mockDuelLogService.Object);
        }

        [Fact]
        public void PlayCard_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            BaseCard card = null;
            Board board = null;

            // Act
            service.PlayCard(card, board);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void PlayMonster_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            Monster monster = null;

            // Act
            service.PlayMonster(monster);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void PlayMonster_StateUnderTest_ExpectedBehavior1()
        {
            // Arrange
            var service = CreateService();
            FieldPosition position = default;
            Board board = null;

            // Act
            service.PlayMonster(position, board);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }
    }
}
