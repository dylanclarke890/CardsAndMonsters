using CardsAndMonsters.Core.Exceptions;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Features.Position;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using CardsAndMonsters.Models.Turns;
using System;
using System.Linq;

namespace CardsAndMonsters.Features.Battle
{
    public class BattleService : IBattleService
    {
        private readonly IDuelLogService _duelLogService;
        public readonly IPositionService _positionService;

        public BattleService(IDuelLogService duelLogService, IPositionService positionService)
        {
            _positionService = positionService;
            _duelLogService = duelLogService;
        }

        public bool DeclaringAttack { get; set; }

        public BattleInfo CurrentBattle { get; set; }

        public void DeclareAttack(BattleInfo info)
        {
            if (info == null)
            {
                throw new GameArgumentException<BattleInfo>(nameof(info), info);
            }

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
                throw new GameArgumentException<BattleInfo>(nameof(CurrentBattle), CurrentBattle);
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
            if (battleInfo == null)
            {
                throw new GameArgumentException<BattleInfo>(nameof(battleInfo), battleInfo);
            }

            switch (battleInfo.Target)
            {
                case BattleTarget.Direct:
                    {
                        if (battleInfo.Board.CurrentTurn.Duelist.Equals(battleInfo.Board.Player))
                        {
                            if (battleInfo.Board.OpponentField.Monsters.Any())
                            {
                                throw new IncorrectMoveException("Attacked directly but opponent has monsters.");
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
                                throw new IncorrectMoveException("Attacked directly but player has monsters.");
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
            if (battleInfo == null)
            {
                throw new GameArgumentException<BattleInfo>(nameof(battleInfo), battleInfo);
            }

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
            if (battleInfo == null)
            {
                throw new GameArgumentException<BattleInfo>(nameof(battleInfo), battleInfo);
            }

            MonsterAttacked(battleInfo.AttackingMonster.Id, battleInfo.Board);
            DamageDuelist(battleInfo.DefendingPlayer, battleInfo.AttackingMonster.Attack, battleInfo.Board);
            battleInfo.Successful = true;
        }

        private static void MonsterAttacked(Guid monsterId, Board board)
        {
            GetMonsterStateByKey(monsterId, board).TimesAttacked++;
        }

        private static void MarkAsDestroyed(Guid monsterId, Board board)
        {
            GetMonsterStateByKey(monsterId, board).Destroyed = true;
        }

        private static MonsterTurnState GetMonsterStateByKey(Guid monsterId, Board board)
        {
            if (monsterId == Guid.Empty)
            {
                throw new GameArgumentException<MonsterTurnState>(nameof(monsterId), monsterId);
            }
            if (board == null)
            {
                throw new GameArgumentException<MonsterTurnState>(nameof(board), board);
            }

            if (board.CurrentTurn.MonsterState.TryGetValue(monsterId, out var value))
            {
                return value;
            }
            else
            {
                throw new IncorrectMoveException($"{monsterId} not found in monster states.");
            }
        }

        private void CalculateAtkVsAtk(BattleInfo battleInfo)
        {
            if (battleInfo == null)
            {
                throw new GameArgumentException<BattleInfo>(nameof(battleInfo), battleInfo);
            }

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
            if (battleInfo == null)
            {
                throw new GameArgumentException<BattleInfo>(nameof(battleInfo), battleInfo);
            }

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
            if (monster == null)
            {
                throw new GameArgumentException<Monster>(nameof(monster), monster);
            }
            if (duelist == null)
            {
                throw new GameArgumentException<Monster>(nameof(duelist), duelist);
            }
            if (board == null)
            {
                throw new GameArgumentException<Monster>(nameof(board), board);
            }

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

        private void DamageDuelist(Duelist player, decimal dmg, Board board)
        {
            if (player == null)
            {
                throw new GameArgumentException<Duelist>(nameof(player), player);
            }
            if (board == null)
            {
                throw new GameArgumentException<Duelist>(nameof(board), board);
            }

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
            if (board == null)
            {
                throw new GameArgumentException<Duelist>(nameof(board), board);
            }

            board.Opponent.TakeDamage(amount);
            _duelLogService.AddNewEventLog(Event.DamageTaken, board.Opponent);
        }

        private void DamagePlayer(decimal amount, Board board)
        {
            if (board == null)
            {
                throw new GameArgumentException<Duelist>(nameof(board), board);
            }

            board.Player.TakeDamage(amount);
            _duelLogService.AddNewEventLog(Event.DamageTaken, board.Player);
        }
    }
}
