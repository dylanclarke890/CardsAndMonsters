using CardsAndMonsters.Core;
using CardsAndMonsters.Data.Factories;
using CardsAndMonsters.Features.Battle;
using CardsAndMonsters.Features.GameOver;
using CardsAndMonsters.Features.Opponent;
using CardsAndMonsters.Features.Position;
using CardsAndMonsters.Features.Turn;
using CardsAndMonsters.Features.TurnPhase;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using System;
using System.Threading.Tasks;

namespace CardsAndMonsters.Features
{
    public class GameService : IGameService, IDisposable
    {
        private readonly IDuelistFactory _duelistFactory;
        private readonly IBattleService _battleService;
        private readonly ITurnService _turnService;
        private readonly IPhaseService _phaseService;
        private readonly IPositionService _positionService;
        private readonly IFakeOpponentService _fakeOpponentService;
        private readonly IGameOverService _gameOverService;
        private bool disposedValue;

        public GameService(IDuelistFactory duelistFactory, IBattleService battleService,
            ITurnService turnService, IPhaseService phaseService, IPositionService positionService,
            IFakeOpponentService fakeOpponentService, IGameOverService gameOverService)
        {
            _duelistFactory = duelistFactory;
            _battleService = battleService;
            _turnService = turnService;
            _phaseService = phaseService;
            _positionService = positionService;
            _fakeOpponentService = fakeOpponentService;
            _gameOverService = gameOverService;

            _phaseService.PhaseChanged += StateHasChanged;
        }

        public Board Board { get; set; }

        public Action OnAction { get; set; }

        public bool ChoosingFieldPosition { get; set; }

        public BaseCard PendingPlacement { get; set; }

        public async Task StartGame()
        {
            Board = new(_duelistFactory.GetNewPlayer(), _duelistFactory.GetNewOpponent());

            Random rnd = new();
            var startingPlayer = rnd.Next(2) == 1 ? Board.Opponent : Board.Player;
            Board.CurrentTurn = new(startingPlayer);

            await EnterPhase(Phase.Standby);

            for (int i = 0; i < AppConstants.HandSize; i++)
            {
                Board.Opponent.DrawCard();
                Board.Player.DrawCard();
                await Task.Delay(300);
            }

            await _turnService.StartTurn(startingPlayer, false, Board);

            if (!startingPlayer.Equals(Board.Player))
            {
                await FakeOpponentsTurn();
            }
        }

        public async Task EnterPhase(Phase phase)
        {
            await _phaseService.EnterPhase(phase, Board);
        }

        public void PlayCard(BaseCard card)
        {
            if (card.IsType(typeof(Monster)))
            {
                var monster = card as Monster;
                if (Board.Player.Equals(Board.CurrentTurn.Player))
                {
                    if (Board.PlayerField.Monsters.Count == AppConstants.FieldSize || Board.CurrentTurn.NormalSummonLimitReached())
                    {
                        return;
                    }

                    PlayMonster(monster);
                }
            }
        }

        public void PlayMonster(Monster monster)
        {
            ChoosingFieldPosition = true;
            PendingPlacement = monster ?? throw new ArgumentException("Monster needs a value to be able to select placement.");
        }

        public void PlayMonster(FieldPosition position)
        {
            ChoosingFieldPosition = false;
            var monster = PendingPlacement as Monster;
            monster.FieldPosition = position;
            Board.Player.PlayMonster(monster, Board, Board.CurrentTurn);
            if (Board.TurnCount == 0)
            {
                Board.CurrentTurn.MonsterState[monster.Id].TimesAttacked = monster.AttacksPerTurn;
            }
            PendingPlacement = null;
        }

        public void Attack(BattleInfo battleInfo)
        {
            _battleService.Attack(battleInfo);
            _gameOverService.CheckForGameOver(Board);
        }

        public void SwitchPosition(Monster monster)
        {
            monster.FieldPosition = _positionService.NewPosition(monster.FieldPosition);
            _positionService.PositionSwitched(monster, Board);
        }

        public async Task EndTurn()
        {
            await _turnService.EndTurn(Board);

            if (Board.CurrentTurn.Player.Equals(Board.Opponent))
            {
                await FakeOpponentsTurn();
            }
        }

        private async Task FakeOpponentsTurn()
        {
            await _fakeOpponentService.FakeMainPhase(Board);
            await _fakeOpponentService.FakeBattlePhase(Board);
            await _fakeOpponentService.FakeEndPhase(Board);
        }

        private void StateHasChanged()
        {
            OnAction?.Invoke();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _phaseService.PhaseChanged -= StateHasChanged;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
