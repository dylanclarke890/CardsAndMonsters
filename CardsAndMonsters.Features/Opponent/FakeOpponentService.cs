using CardsAndMonsters.Core;
using CardsAndMonsters.Features.Battle;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Features.Position;
using CardsAndMonsters.Features.Storage;
using CardsAndMonsters.Features.Turn;
using CardsAndMonsters.Features.TurnPhase;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CardsAndMonsters.Features.Opponent
{
    public class FakeOpponentService : IFakeOpponentService
    {
        private readonly IBattleService _battleService;
        private readonly IPhaseService _phaseService;
        private readonly ITurnService _turnService;
        private readonly IPositionService _positionService;
        private readonly IDuelLogService _duelLogService;
        private readonly IBoardManagementService _boardManagementService;

        public FakeOpponentService(IBattleService battleService, IPhaseService phaseService,
            ITurnService turnService, IPositionService positionService,
            IDuelLogService duelLogService, IBoardManagementService boardManagementService)
        {
            _battleService = battleService;
            _phaseService = phaseService;
            _turnService = turnService;
            _positionService = positionService;
            _duelLogService = duelLogService;
            _boardManagementService = boardManagementService;
        }

        public async Task FakeMainPhase(Board board)
        {
            foreach (var monster in board.OpponentField.Monsters)
            {
                var monsterState = board.CurrentTurn.MonsterState[monster.Id];
                if (monsterState.AbleToSwitch)
                {
                    monster.FieldPosition = _positionService.NewPosition(monster.FieldPosition);
                    monsterState.AbleToSwitch = false;
                    _duelLogService.AddNewEventLog(Event.MonsterPositionChange, board.Opponent);
                }
            }

            Random rnd = new();

            var card = board.Opponent.CurrentHand[rnd.Next(board.Opponent.CurrentHand.Count)];
            if (card.IsType(typeof(Monster)) && board.OpponentField.Monsters.Count < AppConstants.FieldSize
                && !board.CurrentTurn.NormalSummonLimitReached())
            {
                var monster = card as Monster;
                monster.FieldPosition = rnd.Next(2) == 1 ? FieldPosition.VerticalUp : FieldPosition.HorizontalDown;

                board.Opponent.PlayMonster(monster, board, board.CurrentTurn);
                _duelLogService.AddNewEventLog(Event.PlayMonster, board.Opponent);

                if (board.TurnCount == 0)
                {
                    board.CurrentTurn.MonsterState[monster.Id].TimesAttacked = monster.AttacksPerTurn;
                }
            }

            await _boardManagementService.Save(board);
        }

        public async Task FakeBattlePhase(Board board)
        {
            await _phaseService.EnterPhase(Phase.Battle, board);

            Monster[] monsters = new Monster[board.OpponentField.Monsters.Count];
            board.OpponentField.Monsters.CopyTo(monsters, 0);

            foreach (var monster in monsters)
            {
                if (monster.FieldPosition is not FieldPosition.VerticalUp) continue;
                var monsterState = board.CurrentTurn.MonsterState[monster.Id];
                if (monsterState.Destroyed || monsterState.TimesAttacked == monster.AttacksPerTurn) continue;

                BattleInfo battleInfo = new()
                {
                    Board = board,
                    AttackingMonster = monster,
                    AttackingPlayer = board.Opponent,
                    DefendingPlayer = board.Player
                };
                _duelLogService.AddNewEventLog(Event.AttackDeclared, battleInfo.AttackingPlayer);

                if (board.PlayerField.Monsters.Any())
                {
                    battleInfo.Target = BattleTarget.Monster;
                    Random rnd = new();

                    battleInfo.TargetMonster = board.PlayerField.Monsters[rnd.Next(board.PlayerField.Monsters.Count)];
                    _battleService.Attack(battleInfo);
                }
                else
                {
                    battleInfo.Target = BattleTarget.Direct;
                    _battleService.Attack(battleInfo);
                }
            }

            await _boardManagementService.Save(board);
        }

        public async Task FakeEndPhase(Board board)
        {
            await _turnService.EndTurn(board);
            await _boardManagementService.Save(board);
        }

        public async Task ResumePhase(Board board)
        {
            switch (board.CurrentTurn.Phase)
            {
                case Phase.Standby:
                    await _phaseService.EnterPhase(Phase.Main, board);
                    await ResumePhase(board);
                    break;
                case Phase.Main:
                    await FakeMainPhase(board);
                    await _phaseService.EnterPhase(Phase.Battle, board);
                    await ResumePhase(board);
                    break;
                case Phase.Battle:
                    await FakeBattlePhase(board);
                    await _phaseService.EnterPhase(Phase.End, board);
                    await ResumePhase(board);
                    break;
                case Phase.End:
                    await FakeEndPhase(board);
                    break;
                default:
                    throw new ArgumentException("Couldn't figure out the phase");
            }
        }
    }
}
