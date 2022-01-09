using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Turns;
using Xunit;

namespace CardsAndMonsters.Models.Tests
{
    public class DuelistTests
    {
        [Theory]
        [InlineData(-100)]
        [InlineData(0)]
        public void OutOfHealth_HPZeroOrLess_ReturnsTrue(int hp)
        {
            // Arrange
            var duelist = new Duelist() { HP = hp };

            // Act
            var result = duelist.OutOfHealth();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void OutOfHealth_HPMoreThanZero_ReturnsFalse()
        {
            // Arrange
            var duelist = new Duelist() { HP = 100 };

            // Act
            var result = duelist.OutOfHealth();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DrawCard_SuccessfullyDraws_ReturnsTrue()
        {
            // Arrange
            var duelist = new Duelist();
            duelist.Deck.Add(new Monster(100, 100));

            // Act
            var result = duelist.DrawCard();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DrawCard_FailsToDraw_ReturnsFalse()
        {
            // Arrange
            var duelist = new Duelist();

            // Act
            var result = duelist.DrawCard();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void PlayMonster_ValidParameters_RemovesFromHandAndAddsToField()
        {
            // Arrange
            var duelist = new Duelist();
            Monster monster = new(100, 100);
            duelist.CurrentHand.Add(monster);

            Board board = new() { Player = duelist };
            TurnState turn = new(duelist);

            // Act
            duelist.PlayMonster(monster, board, turn);

            // Assert
            Assert.DoesNotContain(monster, duelist.CurrentHand);
            Assert.Contains(monster, board.PlayerField.Monsters);
        }

        [Theory]
        [InlineData(100, 900)]
        [InlineData(1000, 0)]
        [InlineData(1100, -100)]
        public void TakeDamage_DamageCalculation_ReturnsExpectedHP(int dmg, int expectedHP)
        {
            // Arrange
            var duelist = new Duelist() { HP = 1000 };

            // Act
            duelist.TakeDamage(dmg);

            // Assert
            Assert.Equal(expectedHP, duelist.HP);
        }
    }
}
