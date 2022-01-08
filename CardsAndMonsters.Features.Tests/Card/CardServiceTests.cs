using CardsAndMonsters.Features.Card;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using CardsAndMonsters.Models.Turns;
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
        public void PlayCard_ValidMonsterCardPlayed_SetsChoosingFieldPlacementToTrue()
        {
            // Arrange
            var service = CreateService();

            Duelist player = new() { HP = 1000 };
            Duelist opponent = new() { HP = 1000 };

            BaseCard card = new Monster(100, 100);
            TurnState currentTurn = new() { Duelist = player, NormalSummonLimit = 1 };

            Board board = new(player, opponent) { CurrentTurn = currentTurn };

            // Act
            service.PlayCard(card, board);

            // Assert
            Assert.True(service.ChoosingFieldPosition);
            Assert.Same(card, service.PendingPlacement);
            
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void PlayMonster_ValidMonster_SetsChoosingFieldPlacementToTrue()
        {
            // Arrange
            var service = CreateService();
            Monster monster = new(100, 100);

            // Act
            service.PlayMonster(monster);

            // Assert
            Assert.True(service.ChoosingFieldPosition);
            Assert.Same(monster, service.PendingPlacement);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void PlayMonster_ValidPosition_PlaysMonsterInCorrectPosition()
        {
            // Arrange
            Duelist player = new() { HP = 1000 };
            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.PlayMonster, player));

            var service = CreateService();

            FieldPosition position = FieldPosition.HorizontalDown;
            BaseCard card = new Monster(100, 100);
            service.PendingPlacement = card;

            Duelist opponent = new() { HP = 1000 };
            TurnState currentTurn = new() { Duelist = player };
            Board board = new(player, opponent) { CurrentTurn = currentTurn };

            // Act
            service.PlayMonster(position, board);

            // Assert
            Assert.Equal(position, card.FieldPosition);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void PlayMonster_AfterCompleting_ClearsPlacementInfo()
        {
            // Arrange
            Duelist player = new() { HP = 1000 };
            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.PlayMonster, player));

            var service = CreateService();

            FieldPosition position = FieldPosition.HorizontalDown;
            BaseCard card = new Monster(100, 100);
            service.PendingPlacement = card;

            Duelist opponent = new() { HP = 1000 };
            TurnState currentTurn = new() { Duelist = player };
            Board board = new(player, opponent) { CurrentTurn = currentTurn };

            // Act
            service.PlayMonster(position, board);

            // Assert
            Assert.False(service.ChoosingFieldPosition);
            Assert.Null(service.PendingPlacement);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void PlayMonster_OnFirstTurn_SetsMonsterTimesAttackedToItsAttackLimit()
        {
            // Arrange
            Duelist player = new() { HP = 1000 };
            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.PlayMonster, player));

            var service = CreateService();

            FieldPosition position = FieldPosition.HorizontalDown;
            BaseCard card = new Monster(100, 100) { AttacksPerTurn = 2 };
            service.PendingPlacement = card;

            Duelist opponent = new() { HP = 1000 };
            TurnState currentTurn = new() { Duelist = player };
            Board board = new(player, opponent) { CurrentTurn = currentTurn };

            // Act
            service.PlayMonster(position, board);

            // Assert
            Assert.Equal(2, board.CurrentTurn.MonsterState[card.Id].TimesAttacked);

            _mockRepository.VerifyAll();
        }
    }
}
