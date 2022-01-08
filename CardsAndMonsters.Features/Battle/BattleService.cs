using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Features.Position;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using System;
using System.Linq;

namespace CardsAndMonsters.Features.Battle
{
    public class BattleService : IBattleService
    {
        public readonly IPositionService _positionService;
        private readonly IDuelLogService _duelLogService;

        public BattleService(IPositionService positionService,
            IDuelLogService duelLogService)
        {
            _positionService = positionService;
            _duelLogService = duelLogService;
        }

        public bool DeclaringAttack { get; set; }

        public BattleInfo CurrentBattle { get; set; }

        public void DeclareAttack(BattleInfo info)
        {
            CurrentBattle = new()
            {
                Board = info.Board,
                AttackingMonster = info.AttackingMonster,
                AttackingPlayer = info.AttackingPlayer,
                DefendingPlayer = info.DefendingPlayer
            };
            DeclaringAttack = true;
            _duelLogService.AddNewEventLog(Event.AttackDeclared, info.AttackingPlayer);
        }

        public void AttackTarget(BattleInfo info)
        {
            if (CurrentBattle == null)
            {
                throw new ApplicationException("This should've been filled out.");
            }

            CurrentBattle.Target = info.Target;
            CurrentBattle.TargetMonster = info.TargetMonster;
        }

        public void ClearCurrentBattle()
        {
            CurrentBattle = null;
            DeclaringAttack = false;
        }

        public void Attack(BattleInfo battleInfo)
        {
            switch (battleInfo.Target)
            {
                case BattleTarget.Direct:
                    {
                        if (battleInfo.Board.CurrentTurn.Duelist.Equals(battleInfo.Board.Player))
                        {
                            if (battleInfo.Board.OpponentField.Monsters.Any())
                            {
                                throw new InvalidOperationException("Cannot attack directly if duelist has monsters");
                            }
                            else
                            {
                                DirectAttack(battleInfo);
                            }
                        }
                        else
                        {
                            if (battleInfo.Board.PlayerField.Monsters.Any())
                            {
                                throw new InvalidOperationException("Cannot attack directly if duelist has monsters");
                            }
                            else
                            {
                                DirectAttack(battleInfo);
                            }
                        }
                        break;
                    }
                case BattleTarget.Monster:
                    {
                        MonsterAttack(battleInfo);
                        break;
                    }
                default:
                    break;
            }
            battleInfo.Board.CurrentTurn.Battles.Add(battleInfo);
        }

        private void MonsterAttack(BattleInfo battleInfo)
        {
            switch (battleInfo.TargetMonster.FieldPosition)
            {
                case FieldPosition.HorizontalDown:
                    {
                        battleInfo.TargetMonster.FieldPosition = _positionService.NewPosition(battleInfo.TargetMonster.FieldPosition);
                        CalculateAtkVsDef(battleInfo);
                    }
                    break;
                case FieldPosition.HorizontalUp:
                    {
                        CalculateAtkVsDef(battleInfo);
                    }
                    break;
                case FieldPosition.VerticalUp:
                    {
                        CalculateAtkVsAtk(battleInfo);
                    }
                    break;
                default:
                    break;
            }
            MonsterAttacked(battleInfo.AttackingMonster.Id, battleInfo.Board);
        }

        private void DirectAttack(BattleInfo battleInfo)
        {
            MonsterAttacked(battleInfo.AttackingMonster.Id, battleInfo.Board);
            DamageDuelist(battleInfo.DefendingPlayer, battleInfo.AttackingMonster.Attack, battleInfo.Board);
            battleInfo.Successful = true;
        }

        private static void MonsterAttacked(Guid monsterId, Board board)
        {
            board.CurrentTurn.MonsterState[monsterId].TimesAttacked++;
        }

        private void CalculateAtkVsAtk(BattleInfo battleInfo)
        {
            if (battleInfo.AttackingMonster.Attack > battleInfo.TargetMonster.Attack)
            {
                DestroyMonster(battleInfo.TargetMonster, battleInfo.DefendingPlayer, battleInfo.Board);
                decimal dmg = battleInfo.AttackingMonster.Attack - battleInfo.TargetMonster.Attack;
                DamageDuelist(battleInfo.DefendingPlayer, dmg, battleInfo.Board);
                battleInfo.Successful = true;
            }
            else if (battleInfo.AttackingMonster.Attack == battleInfo.TargetMonster.Attack)
            {
                DestroyMonster(battleInfo.AttackingMonster, battleInfo.AttackingPlayer, battleInfo.Board);
                DestroyMonster(battleInfo.TargetMonster, battleInfo.DefendingPlayer, battleInfo.Board);
                battleInfo.Successful = false;
            }
            else
            {
                DestroyMonster(battleInfo.AttackingMonster, battleInfo.AttackingPlayer, battleInfo.Board);
                decimal dmg = battleInfo.TargetMonster.Attack - battleInfo.AttackingMonster.Attack;
                DamageDuelist(battleInfo.AttackingPlayer, dmg, battleInfo.Board);
                battleInfo.Successful = false;
            }
        }

        private void CalculateAtkVsDef(BattleInfo battleInfo)
        {
            if (battleInfo.AttackingMonster.Attack > battleInfo.TargetMonster.Defense)
            {
                DestroyMonster(battleInfo.TargetMonster, battleInfo.DefendingPlayer, battleInfo.Board);
                battleInfo.Successful = true;
            }
            else if (battleInfo.AttackingMonster.Attack == battleInfo.TargetMonster.Defense)
            {
                battleInfo.Successful = false;
            }
            else if (battleInfo.AttackingMonster.Attack < battleInfo.TargetMonster.Defense)
            {
                decimal dmg = battleInfo.TargetMonster.Defense - battleInfo.AttackingMonster.Attack;
                DamageDuelist(battleInfo.AttackingPlayer, dmg, battleInfo.Board);
                battleInfo.Successful = false;
            }
        }

        private void DestroyMonster(Monster monster, Duelist duelist, Board board)
        {
            if (board.Player.Equals(duelist))
            {
                board.PlayerField.Monsters.Remove(monster);
                board.PlayerField.Graveyard.Add(monster);
                if (board.CurrentTurn.Duelist.Equals(board.Player))
                {
                    MarkAsDestroyed(monster.Id, board);
                }
                _duelLogService.AddNewEventLog(Event.MonsterDestroyed, board.Player);
            }
            else
            {
                board.OpponentField.Monsters.Remove(monster);
                board.OpponentField.Graveyard.Add(monster);
                if (board.CurrentTurn.Duelist.Equals(board.Opponent))
                {
                    MarkAsDestroyed(monster.Id, board);
                }
                _duelLogService.AddNewEventLog(Event.MonsterDestroyed, board.Opponent);
            }
        }

        private static void MarkAsDestroyed(Guid monsterId, Board board)
        {
            board.CurrentTurn.MonsterState[monsterId].Destroyed = true;
        }

        private void DamageDuelist(Duelist player, decimal dmg, Board board)
        {
            if (player.Equals(board.Player))
            {
                DamagePlayer(dmg, board);
            }
            else
            {
                DamageOpponent(dmg, board);
            }
        }

        private void DamageOpponent(decimal amount, Board board)
        {
            board.Opponent.TakeDamage(amount);
            _duelLogService.AddNewEventLog(Event.DamageTaken, board.Opponent);
        }

        private void DamagePlayer(decimal amount, Board board)
        {
            board.Player.TakeDamage(amount);
            _duelLogService.AddNewEventLog(Event.DamageTaken, board.Player);
        }
    }
}
