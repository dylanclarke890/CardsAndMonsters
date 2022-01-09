using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using Xunit;

namespace CardsAndMonsters.Models.Tests
{
    public class BoardTests
    {
        [Fact]
        public void AbleToPlayMonster_WhenAbleToPlay_ReturnsTrue()
        {
            // Arrange
            Monster monster = new(100, 100);
            Duelist player = new("test");
            player.CurrentHand.Add(monster);
            Board board = new() { Player = player };
            board.CurrentTurn = new(player) { Phase = Phase.Main };

            // Act
            var result = board.AbleToPlayMonster(monster);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AbleToPlayMonster_WhenHandDoesNotContainMonster_ReturnsFalse()
        {
            // Arrange
            Monster monster = new(100, 100);
            Duelist player = new("test");
            Board board = new() { Player = player };
            board.CurrentTurn = new(player) { Phase = Phase.Main };

            // Act
            var result = board.AbleToPlayMonster(monster);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(Phase.Standby)]
        [InlineData(Phase.Battle)]
        [InlineData(Phase.End)]
        public void AbleToPlayMonster_WhenPhaseIsNotMain_ReturnsFalse(Phase phase)
        {
            // Arrange
            Monster monster = new(100, 100);
            Duelist player = new("test");
            player.CurrentHand.Add(monster);
            Board board = new() { Player = player };
            board.CurrentTurn = new(player) { Phase = phase };

            // Act
            var result = board.AbleToPlayMonster(monster);

            // Assert
            Assert.False(result);
        }
    }
}
