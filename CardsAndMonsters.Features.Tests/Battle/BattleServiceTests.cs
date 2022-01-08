using CardsAndMonsters.Features.Battle;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Features.Position;
using CardsAndMonsters.Models;
using Moq;
using Xunit;

namespace CardsAndMonsters.Features.Tests.Battle
{
    public class BattleServiceTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IPositionService> _mockPositionService;
        private readonly Mock<IDuelLogService> _mockDuelLogService;

        public BattleServiceTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockPositionService = _mockRepository.Create<IPositionService>();
            _mockDuelLogService = _mockRepository.Create<IDuelLogService>();
        }

        private BattleService CreateService()
        {
            return new BattleService(_mockPositionService.Object, _mockDuelLogService.Object);
        }

        [Fact]
        public void DeclareAttack_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            BattleInfo info = null;

            // Act
            service.DeclareAttack(info);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void AttackTarget_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            BattleInfo info = null;

            // Act
            service.AttackTarget(info);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void ClearCurrentBattle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();

            // Act
            service.ClearCurrentBattle();

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Attack_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            BattleInfo battleInfo = null;

            // Act
            service.Attack(battleInfo);

            // Assert
            Assert.True(false);
            _mockRepository.VerifyAll();
        }
    }
}
