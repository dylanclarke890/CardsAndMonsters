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

        public BattleService(IPositionService positionService)
        {
            _positionService = positionService;
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
                        if (battleInfo.Board.OpponentField.Monsters.Any())
                        {
                            throw new InvalidOperationException("Cannot attack directly if opponent has monsters");
                        }
                        else
                        {
                            DirectAttack(battleInfo);
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
        }

        private void MonsterAttack(BattleInfo battleInfo)
        {
            switch (battleInfo.TargetMonster.FieldPosition)
            {
                case FieldPosition.HorizontalUp:
                    {
                        battleInfo.TargetMonster.FieldPosition = _positionService.NewPosition(battleInfo.TargetMonster.FieldPosition);
                        CalculateAtkVsDef(battleInfo);
                    }
                    break;
                case FieldPosition.HorizontalDown:
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

        private static void DirectAttack(BattleInfo battleInfo)
        {
            MonsterAttacked(battleInfo.AttackingMonster.Id, battleInfo.Board);
            DamageDuelist(battleInfo.DefendingPlayer, battleInfo.AttackingMonster.Attack, battleInfo.Board);
        }

        private static void MonsterAttacked(Guid monsterId, Board board)
        {
            board.CurrentTurn.MonsterState[monsterId].TimesAttacked++;
        }

        private static void CalculateAtkVsAtk(BattleInfo battleInfo)
        {
            if (battleInfo.AttackingMonster.Attack > battleInfo.TargetMonster.Attack)
            {
                DestroyMonster(battleInfo.TargetMonster, battleInfo.DefendingPlayer, battleInfo.Board);
                decimal dmg = battleInfo.AttackingMonster.Attack - battleInfo.TargetMonster.Attack;
                DamageDuelist(battleInfo.DefendingPlayer, dmg, battleInfo.Board);
            }
            else if (battleInfo.AttackingMonster.Attack == battleInfo.TargetMonster.Attack)
            {
                DestroyMonster(battleInfo.AttackingMonster, battleInfo.AttackingPlayer, battleInfo.Board);
                DestroyMonster(battleInfo.TargetMonster, battleInfo.DefendingPlayer, battleInfo.Board);
            }
            else
            {
                DestroyMonster(battleInfo.AttackingMonster, battleInfo.AttackingPlayer, battleInfo.Board);
                decimal dmg = battleInfo.TargetMonster.Attack - battleInfo.AttackingMonster.Attack;
                DamageDuelist(battleInfo.AttackingPlayer, dmg, battleInfo.Board);
            }
        }

        private static void CalculateAtkVsDef(BattleInfo battleInfo)
        {
            if (battleInfo.AttackingMonster.Attack > battleInfo.TargetMonster.Defense)
            {
                DestroyMonster(battleInfo.TargetMonster, battleInfo.DefendingPlayer, battleInfo.Board);
            }
            else if (battleInfo.AttackingMonster.Attack < battleInfo.TargetMonster.Defense)
            {
                DestroyMonster(battleInfo.AttackingMonster, battleInfo.AttackingPlayer, battleInfo.Board);
                decimal dmg = battleInfo.TargetMonster.Defense - battleInfo.AttackingMonster.Attack;
                DamageDuelist(battleInfo.AttackingPlayer, dmg, battleInfo.Board);
            }
        }

        private static void DestroyMonster(Monster monster, Duelist player, Board board)
        {
            if (board.Player.Equals(player))
            {
                board.PlayerField.Monsters.Remove(monster);
                board.PlayerField.Graveyard.Add(monster);
                if (board.CurrentTurn.Player.Equals(board.Player))
                {
                    MarkAsDestroyed(monster.Id, board);
                }
            }
            else
            {
                board.OpponentField.Monsters.Remove(monster);
                board.OpponentField.Graveyard.Add(monster);
                if (board.CurrentTurn.Player.Equals(board.Opponent))
                {
                    MarkAsDestroyed(monster.Id, board);
                }
            }
        }

        private static void MarkAsDestroyed(Guid monsterId, Board board)
        {
            board.CurrentTurn.MonsterState[monsterId].Destroyed = true;
        }

        private static void DamageDuelist(Duelist player, decimal dmg, Board board)
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

        private static void DamageOpponent(decimal amount, Board board)
        {
            board.Opponent.TakeDamage(amount);
        }

        private static void DamagePlayer(decimal amount, Board board)
        {
            board.Player.TakeDamage(amount);
        }
    }
}
