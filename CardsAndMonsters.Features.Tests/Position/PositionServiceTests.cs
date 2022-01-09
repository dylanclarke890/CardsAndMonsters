using CardsAndMonsters.Core.Exceptions;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Features.Position;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using CardsAndMonsters.Models.Turns;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace CardsAndMonsters.Features.Tests.Position
{
    public class PositionServiceTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IDuelLogService> _mockDuelLogService;

        public PositionServiceTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockDuelLogService = _mockRepository.Create<IDuelLogService>();
        }

        private PositionService CreateService()
        {
            return new PositionService(_mockDuelLogService.Object);
        }

        [Fact]
        public void NewPosition_ValidPosition_ReturnsCorrectPosition()
        {
            // Arrange
            var service = CreateService();
            FieldPosition old = FieldPosition.HorizontalDown;

            // Act
            var result = service.NewPosition(old);

            // Assert
            Assert.Equal(FieldPosition.VerticalUp, result);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void NewPosition_InvalidPosition_ThrowsGameArgumentException()
        {
            // Arrange
            var service = CreateService();
            FieldPosition old = (FieldPosition)1000;

            // Act
            FieldPosition act() => service.NewPosition(old);

            // Assert
            Assert.Throws<GameArgumentException<BaseCard>>(() => act());

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void PositionSwitched_ValidParameters_SetsAbleToSwitchToFalse()
        {
            // Arrange
            Duelist player = new("test");
            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.MonsterPositionChange, player));

            var service = CreateService();

            Monster monster = new(100, 100);
            Board board = new() { Player = player };
            board.PlayerField.Monsters.Add(monster);
            board.CurrentTurn = new()
            {
                MonsterState = new Dictionary<Guid, MonsterTurnState>()
                {
                    [monster.Id] = new() { Monster = monster }
                }

            };

            // Act
            service.PositionSwitched(monster, board);

            // Assert
            Assert.False(board.CurrentTurn.MonsterState[monster.Id].AbleToSwitch);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void PositionSwitched_NullMonster_ThrowsGameArgumentException()
        {
            // Arrange
            var service = CreateService();
            Monster monster = null;
            Board board = new();

            // Act
            void act() => service.PositionSwitched(monster, board);

            // Assert
            Assert.Throws<GameArgumentException<Monster>>(act);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void PositionSwitched_NullBoard_ThrowsGameArgumentException()
        {
            // Arrange
            var service = CreateService();
            Monster monster = new(100, 100);
            Board board = null;

            // Act
            void act() => service.PositionSwitched(monster, board);

            // Assert
            Assert.Throws<GameArgumentException<Monster>>(act);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void PositionSwitched_PlayerFieldDoesNotContainMonster_ThrowsGameArgumentException()
        {
            // Arrange
            var service = CreateService();
            Monster monster = new(100, 100);
            Board board = new();

            // Act
            void act() => service.PositionSwitched(monster, board);

            // Assert
            Assert.Throws<GameArgumentException<Monster>>(act);

            _mockRepository.VerifyAll();
        }
    }
}
