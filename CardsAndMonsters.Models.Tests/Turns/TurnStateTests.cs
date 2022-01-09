using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using CardsAndMonsters.Models.Turns;
using System;
using System.Collections.Generic;
using Xunit;

namespace CardsAndMonsters.Models.Tests.Turns
{
    public class TurnStateTests
    {
        [Fact]
        public void NormalSummonLimitReached_WhenOverLimit_ReturnsTrue()
        {
            // Arrange
            TurnState turnState = new()
            {
                NormalSummonLimit = 1,
                SummonedThisTurn = new List<Monster>() { new(100, 100) }
            };

            // Act
            var result = turnState.NormalSummonLimitReached();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void NormalSummonLimitReached_WhenUnderLimit_ReturnsFalse()
        {
            // Arrange
            TurnState turnState = new()
            {
                NormalSummonLimit = 1,
            };

            // Act
            var result = turnState.NormalSummonLimitReached();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AbleToSwitch_WhenAbleToSwitch_ReturnsTrue()
        {
            // Arrange
            Duelist duelist = new("test");
            Monster monster = new(100, 100);
            Guid monsterId = monster.Id;

            TurnState turnState = new(duelist) { Phase = Phase.Main };
            turnState.MonsterState.Add(monster.Id, new() { Monster = monster, AbleToSwitch = true });

            // Act
            var result = turnState.AbleToSwitch(monsterId, duelist);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(Phase.Standby)]
        [InlineData(Phase.Battle)]
        [InlineData(Phase.End)]
        public void AbleToSwitch_WhenPhaseIsNotMain_ReturnsFalse(Phase phase)
        {
            // Arrange
            Duelist duelist = new("test");
            Monster monster = new(100, 100);
            Guid monsterId = monster.Id;

            TurnState turnState = new(duelist) { Phase = phase };
            turnState.MonsterState.Add(monster.Id, new() { Monster = monster, AbleToSwitch = true });

            // Act
            var result = turnState.AbleToSwitch(monsterId, duelist);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AbleToSwitch_WhenAbleToSwitchIsFalse_ReturnsFalse()
        {
            // Arrange
            Duelist duelist = new("test");
            Monster monster = new(100, 100);
            Guid monsterId = monster.Id;

            TurnState turnState = new(duelist) { Phase = Phase.Main };
            turnState.MonsterState.Add(monster.Id, new() { Monster = monster, AbleToSwitch = false });

            // Act
            var result = turnState.AbleToSwitch(monsterId, duelist);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AbleToSwitch_WhenMonsterStateDoesNotContainMonster_ReturnsFalse()
        {
            // Arrange
            Duelist duelist = new("test");
            Monster monster = new(100, 100);
            Guid monsterId = monster.Id;

            TurnState turnState = new(duelist) { Phase = Phase.Main };

            // Act
            var result = turnState.AbleToSwitch(monsterId, duelist);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AbleToSwitch_WhenDuelistIsNotEqual_ReturnsFalse()
        {
            // Arrange
            Duelist duelist = new("p1");
            Duelist notEqualDuelist = new("p2");
            Monster monster = new(100, 100);
            Guid monsterId = monster.Id;

            TurnState turnState = new(notEqualDuelist) { Phase = Phase.Main };
            turnState.MonsterState.Add(monster.Id, new() { Monster = monster, AbleToSwitch = true });

            // Act
            var result = turnState.AbleToSwitch(monsterId, duelist);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AbleToBattle_WhenAbleToBattle_ReturnsTrue()
        {
            // Arrange
            Duelist duelist = new("p1");
            Monster monster = new(100, 100)
            {
                FieldPosition = FieldPosition.VerticalUp,
                AttacksPerTurn = 1
            };

            Guid monsterId = monster.Id;
            TurnState turnState = new(duelist) { Phase = Phase.Battle };
            turnState.MonsterState.Add(monster.Id, new() { Monster = monster, TimesAttacked = 0 });
            bool declaringAttack = false;

            // Act
            var result = turnState.AbleToBattle(monsterId, duelist, declaringAttack);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AbleToBattle_WhenMaxAttacksReached_ReturnsFalse()
        {
            // Arrange
            Duelist duelist = new("p1");
            Monster monster = new(100, 100)
            {
                FieldPosition = FieldPosition.VerticalUp,
                AttacksPerTurn = 1
            };

            Guid monsterId = monster.Id;
            TurnState turnState = new(duelist) { Phase = Phase.Battle };
            turnState.MonsterState.Add(monster.Id, new() { Monster = monster, TimesAttacked = 1 });
            bool declaringAttack = false;

            // Act
            var result = turnState.AbleToBattle(monsterId, duelist, declaringAttack);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AbleToBattle_WhenMonsterStateDoesNotContainMonster_ReturnsFalse()
        {
            // Arrange
            Duelist duelist = new("p1");
            Monster monster = new(100, 100)
            {
                FieldPosition = FieldPosition.VerticalUp,
                AttacksPerTurn = 1
            };

            Guid monsterId = monster.Id;
            TurnState turnState = new(duelist) { Phase = Phase.Battle };
            bool declaringAttack = false;

            // Act
            var result = turnState.AbleToBattle(monsterId, duelist, declaringAttack);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AbleToBattle_WhenDeclaringAttack_ReturnsTrue()
        {
            // Arrange
            Duelist duelist = new("p1");
            Monster monster = new(100, 100)
            {
                FieldPosition = FieldPosition.VerticalUp,
                AttacksPerTurn = 1
            };

            Guid monsterId = monster.Id;
            TurnState turnState = new(duelist) { Phase = Phase.Battle };
            turnState.MonsterState.Add(monster.Id, new() { Monster = monster, TimesAttacked = 0 });
            bool declaringAttack = true;

            // Act
            var result = turnState.AbleToBattle(monsterId, duelist, declaringAttack);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(FieldPosition.HorizontalUp)]
        [InlineData(FieldPosition.HorizontalDown)]
        [InlineData(FieldPosition.VerticalDown)]
        public void AbleToBattle_WhenPositionIsNotVerticalUp_ReturnsFalse(FieldPosition position)
        {
            // Arrange
            Duelist duelist = new("p1");
            Monster monster = new(100, 100)
            {
                FieldPosition = position,
                AttacksPerTurn = 1
            };

            Guid monsterId = monster.Id;
            TurnState turnState = new(duelist) { Phase = Phase.Battle };
            turnState.MonsterState.Add(monster.Id, new() { Monster = monster, TimesAttacked = 0 });
            bool declaringAttack = false;

            // Act
            var result = turnState.AbleToBattle(monsterId, duelist, declaringAttack);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(Phase.Standby)]
        [InlineData(Phase.Main)]
        [InlineData(Phase.End)]
        public void AbleToBattle_WhenPhaseIsNotBattle_ReturnsFalse(Phase phase)
        {
            // Arrange
            Duelist duelist = new("p1");
            Monster monster = new(100, 100)
            {
                FieldPosition = FieldPosition.VerticalUp,
                AttacksPerTurn = 1
            };

            Guid monsterId = monster.Id;
            TurnState turnState = new(duelist) { Phase = phase };
            turnState.MonsterState.Add(monster.Id, new() { Monster = monster, TimesAttacked = 0 });
            bool declaringAttack = false;

            // Act
            var result = turnState.AbleToBattle(monsterId, duelist, declaringAttack);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AbleToAttack_WhenAbleToAttack_ReturnsTrue()
        {
            // Arrange
            Duelist duelist = new("p1");
            TurnState turnState = new(duelist) { Phase = Phase.Battle };
            Guid monsterId = Guid.NewGuid();
            bool declaringAttack = true;

            // Act
            var result = turnState.AbleToAttack(monsterId, duelist, declaringAttack);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AbleToAttack_WhenNotDeclaringAttack_ReturnsFalse()
        {
            // Arrange
            Duelist duelist = new("p1");
            TurnState turnState = new(duelist) { Phase = Phase.Battle };
            Guid monsterId = Guid.NewGuid();
            bool declaringAttack = false;

            // Act
            var result = turnState.AbleToAttack(monsterId, duelist, declaringAttack);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AbleToAttack_WhenMonsterStateContainsMonster_ReturnsFalse()
        {
            // Arrange
            Duelist duelist = new("p1");
            TurnState turnState = new(duelist) { Phase = Phase.Battle };
            Guid monsterId = Guid.NewGuid();
            turnState.MonsterState.Add(monsterId, new());
            bool declaringAttack = true;

            // Act
            var result = turnState.AbleToAttack(monsterId, duelist, declaringAttack);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(Phase.Standby)]
        [InlineData(Phase.Main)]
        [InlineData(Phase.End)]
        public void AbleToAttack_WhenPhaseIsNotBattle_ReturnsFalse(Phase phase)
        {
            // Arrange
            Duelist duelist = new("p1");
            TurnState turnState = new(duelist) { Phase = phase };
            Guid monsterId = Guid.NewGuid();
            bool declaringAttack = true;

            // Act
            var result = turnState.AbleToAttack(monsterId, duelist, declaringAttack);

            // Assert
            Assert.False(result);
        }
    }
}
