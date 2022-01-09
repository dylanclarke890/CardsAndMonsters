using CardsAndMonsters.Core;
using CardsAndMonsters.Core.Exceptions;
using CardsAndMonsters.Features.Battle;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Features.Position;
using CardsAndMonsters.Features.RandomNumber;
using CardsAndMonsters.Features.Storage;
using CardsAndMonsters.Features.Turn;
using CardsAndMonsters.Features.TurnPhase;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace CardsAndMonsters.Features.Opponent
{
    public class FakeOpponentService : IFakeOpponentService
    {
        private readonly IBattleService _battleService;
        private readonly IBoardManagementService _boardManagementService;
        private readonly IDuelLogService _duelLogService;
        private readonly INumberGenerator _numberGenerator;
        private readonly IPhaseService _phaseService;
        private readonly IPositionService _positionService;
        private readonly ITurnService _turnService;

        public FakeOpponentService(IBattleService battleService, IBoardManagementService boardManagementService,
             IDuelLogService duelLogService, INumberGenerator numberGenerator, IPhaseService phaseService,
             IPositionService positionService, ITurnService turnService)
        {
            _battleService = battleService;
            _boardManagementService = boardManagementService;
            _duelLogService = duelLogService;
            _numberGenerator = numberGenerator;
            _phaseService = phaseService;
            _positionService = positionService;
            _turnService = turnService;
        }

        public async Task FakeMainPhase(Board board)
        {
            if (board == null)
            {
                throw new GameArgumentException<Board>(nameof(board), board);
            }

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

            if (!board.Opponent.CurrentHand.Any())
            {
                await _boardManagementService.Save(board);
                return;
            }

            var card = board.Opponent.CurrentHand[_numberGenerator.GetRandom(board.Opponent.CurrentHand.Count)];
            if (card.IsType(typeof(Monster)) && board.OpponentField.Monsters.Count < AppConstants.FieldSize
                && !board.CurrentTurn.NormalSummonLimitReached())
            {
                var monster = card as Monster;
                monster.FieldPosition = _numberGenerator.GetRandom(2) == 1 ? FieldPosition.VerticalUp : FieldPosition.HorizontalDown;

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
            if (board == null)
            {
                throw new GameArgumentException<Board>(nameof(board), board);
            }

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

                    battleInfo.TargetMonster = board.PlayerField.Monsters[_numberGenerator.GetRandom(board.PlayerField.Monsters.Count)];
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
            if (board == null)
            {
                throw new GameArgumentException<Board>(nameof(board), board);
            }

            await _turnService.EndTurn(board);
            await _boardManagementService.Save(board);
        }

        public async Task ResumePhase(Board board)
        {
            if (board == null)
            {
                throw new GameArgumentException<Board>(nameof(board), board);
            }

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
                    throw new GameArgumentException<Board>(nameof(board.CurrentTurn.Phase), board.CurrentTurn.Phase);
            }
        }
    }
}
