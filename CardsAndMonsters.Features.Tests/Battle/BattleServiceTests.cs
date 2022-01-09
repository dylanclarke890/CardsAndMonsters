using CardsAndMonsters.Features.Battle;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Features.Position;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using CardsAndMonsters.Models.Turns;
using Moq;
using Xunit;

namespace CardsAndMonsters.Features.Tests.Battle
{
    public class BattleServiceTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IDuelLogService> _mockDuelLogService;
        private readonly Mock<IPositionService> _mockPositionService;

        public BattleServiceTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockDuelLogService = _mockRepository.Create<IDuelLogService>();
            _mockPositionService = _mockRepository.Create<IPositionService>();
        }

        private BattleService CreateService()
        {
            return new BattleService(_mockDuelLogService.Object, _mockPositionService.Object);
        }

        [Fact]
        public void DeclareAttack_ExpectedParameters_CreatesExpectedBattleInfoAndLog()
        {
            // Arrange
            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.AttackDeclared, It.IsAny<Duelist>()));

            var service = CreateService();
            
            Duelist attacker = new();
            Duelist defender = new();
            Monster monster = new(1000, 1000);
            Board board = new();
            BattleInfo info = new()
            {
                AttackingMonster = monster,
                AttackingPlayer = attacker,
                Board = board,
                DefendingPlayer = defender,
            };

            // Act
            service.DeclareAttack(info);

            // Assert
            Assert.True(service.DeclaringAttack);
            
            Assert.Equal(monster, service.CurrentBattle.AttackingMonster);
            Assert.Equal(attacker, service.CurrentBattle.AttackingPlayer);
            Assert.Equal(board, service.CurrentBattle.Board);
            Assert.Equal(defender, service.CurrentBattle.DefendingPlayer);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void AttackTarget_CorrectParameters_AddsInfoToCurrentBattle()
        {
            // Arrange
            var service = CreateService();
            service.CurrentBattle = new();

            BattleTarget target = BattleTarget.Monster;
            Monster targetMonster = new(100, 100);
            BattleInfo info = new() 
            { 
                Target = target, 
                TargetMonster = targetMonster 
            };

            // Act
            service.AttackTarget(info);

            // Assert
            Assert.Equal(target, service.CurrentBattle.Target);
            Assert.Equal(targetMonster, service.CurrentBattle.TargetMonster);
            
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void ClearCurrentBattle_ClearsBattleInfo()
        {
            // Arrange
            var service = CreateService();
            service.CurrentBattle = new()
            {
                AttackingMonster = new(100, 100),
                AttackingPlayer = new(),
                Board = new(),
                DefendingPlayer = new(),
                Target = BattleTarget.Direct,
                TargetMonster = new(100, 100)
            };

            // Act
            service.ClearCurrentBattle();

            // Assert
            Assert.False(service.DeclaringAttack);
            Assert.Null(service.CurrentBattle);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Attack_DefenderDirectly_DamagesDefender()
        {
            // Arrange
            Duelist player = new() { HP = 1000 };
            Duelist opponent = new() { HP = 1000 };

            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.DamageTaken, opponent));

            var service = CreateService();
            
            TurnState currentTurn = new() { Duelist = player };
            Monster monster = new(100, 0);
            MonsterTurnState monsterState = new() { Monster = monster };
            currentTurn.MonsterState.Add(monster.Id, monsterState);
            
            Board board = new(player, opponent) { CurrentTurn = currentTurn };
            board.PlayerField.Monsters.Add(monster);

            BattleInfo battleInfo = new()
            {
                AttackingMonster = monster,
                AttackingPlayer = player,
                Board = board,
                DefendingPlayer = opponent,
                Target = BattleTarget.Direct,
            };

            // Act
            service.Attack(battleInfo);

            // Assert
            Assert.Single(board.PlayerField.Monsters);
            Assert.Equal(900, opponent.HP);
            Assert.Equal(1, board.CurrentTurn.MonsterState[monster.Id].TimesAttacked);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Attack_DefendingMonsterInAttackModeWithHigherValues_DestroysAttackingMonsterAndDamagesAttacker()
        {
            // Arrange
            Duelist player = new() { HP = 1000 };
            Duelist opponent = new() { HP = 1000 };

            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.DamageTaken, player));
            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.MonsterDestroyed, player));

            var service = CreateService();

            TurnState currentTurn = new() { Duelist = player };
            Monster attackingMonster = new(100, 0);
            Monster defendingMonster = new(200, 0);
            MonsterTurnState monsterState = new() { Monster = attackingMonster };
            currentTurn.MonsterState.Add(attackingMonster.Id, monsterState);

            Board board = new(player, opponent) { CurrentTurn = currentTurn };
            board.PlayerField.Monsters.Add(attackingMonster);
            board.OpponentField.Monsters.Add(defendingMonster);

            BattleInfo battleInfo = new()
            {
                AttackingMonster = attackingMonster,
                AttackingPlayer = player,
                Board = board,
                DefendingPlayer = opponent,
                Target = BattleTarget.Monster,
                TargetMonster = defendingMonster
            };

            // Act
            service.Attack(battleInfo);

            // Assert
            Assert.Empty(board.PlayerField.Monsters);
            Assert.Single(board.OpponentField.Monsters);
            Assert.True(board.CurrentTurn.MonsterState[attackingMonster.Id].Destroyed);
            Assert.Equal(900, player.HP);
            Assert.Equal(1, board.CurrentTurn.MonsterState[attackingMonster.Id].TimesAttacked);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Attack_DefendingMonsterInAttackModeWithLowerValues_DestroysDefendingMonsterAndDamagesDefender()
        {
            // Arrange
            Duelist player = new() { HP = 1000 };
            Duelist opponent = new() { HP = 1000 };

            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.DamageTaken, opponent));
            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.MonsterDestroyed, opponent));

            var service = CreateService();

            TurnState currentTurn = new() { Duelist = player };
            Monster attackingMonster = new(200, 0);
            Monster defendingMonster = new(100, 0);
            MonsterTurnState monsterState = new() { Monster = attackingMonster };
            currentTurn.MonsterState.Add(attackingMonster.Id, monsterState);

            Board board = new(player, opponent) { CurrentTurn = currentTurn };
            board.PlayerField.Monsters.Add(attackingMonster);
            board.OpponentField.Monsters.Add(defendingMonster);

            BattleInfo battleInfo = new()
            {
                AttackingMonster = attackingMonster,
                AttackingPlayer = player,
                Board = board,
                DefendingPlayer = opponent,
                Target = BattleTarget.Monster,
                TargetMonster = defendingMonster
            };

            // Act
            service.Attack(battleInfo);

            // Assert
            Assert.Empty(board.OpponentField.Monsters);
            Assert.Equal(900, opponent.HP);
            Assert.Equal(1, board.CurrentTurn.MonsterState[attackingMonster.Id].TimesAttacked);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Attack_DefendingMonsterInAttackModeWithEqualValues_DestroysBothMonsters()
        {
            // Arrange
            Duelist player = new() { HP = 1000 };
            Duelist opponent = new() { HP = 1000 };

            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.MonsterDestroyed, opponent));
            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.MonsterDestroyed, player));

            var service = CreateService();

            TurnState currentTurn = new() { Duelist = player };
            Monster attackingMonster = new(200, 0);
            Monster defendingMonster = new(200, 0);
            MonsterTurnState monsterState = new() { Monster = attackingMonster };
            currentTurn.MonsterState.Add(attackingMonster.Id, monsterState);

            Board board = new(player, opponent) { CurrentTurn = currentTurn };
            board.PlayerField.Monsters.Add(attackingMonster);
            board.OpponentField.Monsters.Add(defendingMonster);

            BattleInfo battleInfo = new()
            {
                AttackingMonster = attackingMonster,
                AttackingPlayer = player,
                Board = board,
                DefendingPlayer = opponent,
                Target = BattleTarget.Monster,
                TargetMonster = defendingMonster
            };

            // Act
            service.Attack(battleInfo);

            // Assert
            Assert.Empty(board.PlayerField.Monsters);
            Assert.Empty(board.OpponentField.Monsters);
            Assert.Equal(1000, player.HP);
            Assert.Equal(1000, opponent.HP);
            Assert.True(board.CurrentTurn.MonsterState[attackingMonster.Id].Destroyed);
            Assert.Equal(1, board.CurrentTurn.MonsterState[attackingMonster.Id].TimesAttacked);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Attack_DefendingMonsterInDefenseModeWithHigherValues_DamagesAttacker()
        {
            // Arrange
            Duelist player = new() { HP = 1000 };
            Duelist opponent = new() { HP = 1000 };

            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.DamageTaken, player));

            var service = CreateService();

            TurnState currentTurn = new() { Duelist = player };
            Monster attackingMonster = new(100, 0);
            Monster defendingMonster = new(0, 200) { FieldPosition = FieldPosition.HorizontalUp };
            MonsterTurnState monsterState = new() { Monster = attackingMonster };
            currentTurn.MonsterState.Add(attackingMonster.Id, monsterState);

            Board board = new(player, opponent) { CurrentTurn = currentTurn };
            board.PlayerField.Monsters.Add(attackingMonster);
            board.OpponentField.Monsters.Add(defendingMonster);

            BattleInfo battleInfo = new()
            {
                AttackingMonster = attackingMonster,
                AttackingPlayer = player,
                Board = board,
                DefendingPlayer = opponent,
                Target = BattleTarget.Monster,
                TargetMonster = defendingMonster
            };

            // Act
            service.Attack(battleInfo);

            // Assert
            Assert.Single(board.PlayerField.Monsters);
            Assert.Single(board.OpponentField.Monsters);
            Assert.False(board.CurrentTurn.MonsterState[attackingMonster.Id].Destroyed);
            Assert.Equal(900, player.HP);
            Assert.Equal(1000, opponent.HP);
            Assert.Equal(1, board.CurrentTurn.MonsterState[attackingMonster.Id].TimesAttacked);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Attack_DefendingMonsterInDefenseModeWithLowerValues_DestroysDefendingMonster()
        {
            // Arrange
            Duelist player = new() { HP = 1000 };
            Duelist opponent = new() { HP = 1000 };

            _mockDuelLogService.Setup(dls => dls.AddNewEventLog(Event.MonsterDestroyed, opponent));

            var service = CreateService();

            TurnState currentTurn = new() { Duelist = player };
            Monster attackingMonster = new(200, 0);
            Monster defendingMonster = new(0, 100) { FieldPosition = FieldPosition.HorizontalUp };
            MonsterTurnState monsterState = new() { Monster = attackingMonster };
            currentTurn.MonsterState.Add(attackingMonster.Id, monsterState);

            Board board = new(player, opponent) { CurrentTurn = currentTurn };
            board.PlayerField.Monsters.Add(attackingMonster);
            board.OpponentField.Monsters.Add(defendingMonster);

            BattleInfo battleInfo = new()
            {
                AttackingMonster = attackingMonster,
                AttackingPlayer = player,
                Board = board,
                DefendingPlayer = opponent,
                Target = BattleTarget.Monster,
                TargetMonster = defendingMonster
            };

            // Act
            service.Attack(battleInfo);

            // Assert
            Assert.Empty(board.OpponentField.Monsters);
            Assert.Equal(1000, opponent.HP);
            Assert.Equal(1000, player.HP);
            Assert.Equal(1, board.CurrentTurn.MonsterState[attackingMonster.Id].TimesAttacked);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Attack_DefendingMonsterInDefenseModeWithEqualValues_NoDamageTakenOrMonstersDestroyed()
        {
            // Arrange
            Duelist player = new() { HP = 1000 };
            Duelist opponent = new() { HP = 1000 };

            var service = CreateService();

            TurnState currentTurn = new() { Duelist = player };
            Monster attackingMonster = new(200, 0);
            Monster defendingMonster = new(0, 200) { FieldPosition = FieldPosition.HorizontalUp };
            MonsterTurnState monsterState = new() { Monster = attackingMonster };
            currentTurn.MonsterState.Add(attackingMonster.Id, monsterState);

            Board board = new(player, opponent) { CurrentTurn = currentTurn };
            board.PlayerField.Monsters.Add(attackingMonster);
            board.OpponentField.Monsters.Add(defendingMonster);

            BattleInfo battleInfo = new()
            {
                AttackingMonster = attackingMonster,
                AttackingPlayer = player,
                Board = board,
                DefendingPlayer = opponent,
                Target = BattleTarget.Monster,
                TargetMonster = defendingMonster
            };

            // Act
            service.Attack(battleInfo);

            // Assert
            Assert.Single(board.PlayerField.Monsters);
            Assert.Single(board.OpponentField.Monsters);
            Assert.Equal(1000, player.HP);
            Assert.Equal(1000, opponent.HP);
            Assert.Equal(1, board.CurrentTurn.MonsterState[attackingMonster.Id].TimesAttacked);
            Assert.False(board.CurrentTurn.MonsterState[attackingMonster.Id].Destroyed);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Attack_DefendingMonsterInFaceDownDefenseModeWithEqualValues_SwitchesDefendingMonsterToFaceUpDefense()
        {
            // Arrange
            Duelist player = new() { HP = 1000 };
            Duelist opponent = new() { HP = 1000 };

            _mockPositionService.Setup(ps => ps.NewPosition(FieldPosition.HorizontalDown))
                .Returns(FieldPosition.HorizontalUp);

            var service = CreateService();

            TurnState currentTurn = new() { Duelist = player };
            Monster attackingMonster = new(200, 0);
            Monster defendingMonster = new(0, 200) { FieldPosition = FieldPosition.HorizontalDown };
            MonsterTurnState monsterState = new() { Monster = attackingMonster };
            currentTurn.MonsterState.Add(attackingMonster.Id, monsterState);

            Board board = new(player, opponent) { CurrentTurn = currentTurn };
            board.PlayerField.Monsters.Add(attackingMonster);
            board.OpponentField.Monsters.Add(defendingMonster);

            BattleInfo battleInfo = new()
            {
                AttackingMonster = attackingMonster,
                AttackingPlayer = player,
                Board = board,
                DefendingPlayer = opponent,
                Target = BattleTarget.Monster,
                TargetMonster = defendingMonster
            };

            // Act
            service.Attack(battleInfo);

            // Assert
            Assert.Single(board.PlayerField.Monsters);
            Assert.Single(board.OpponentField.Monsters);
            Assert.Equal(1000, player.HP);
            Assert.Equal(1000, opponent.HP);
            Assert.Equal(FieldPosition.HorizontalUp, defendingMonster.FieldPosition);
            Assert.Equal(1, board.CurrentTurn.MonsterState[attackingMonster.Id].TimesAttacked);
            Assert.False(board.CurrentTurn.MonsterState[attackingMonster.Id].Destroyed);

            _mockRepository.VerifyAll();
        }
    }
}
