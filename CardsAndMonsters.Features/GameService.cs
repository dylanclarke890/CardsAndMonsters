using CardsAndMonsters.Core;
using CardsAndMonsters.Data.Factories;
using CardsAndMonsters.Features.Battle;
using CardsAndMonsters.Features.Card;
using CardsAndMonsters.Features.GameOver;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Features.Opponent;
using CardsAndMonsters.Features.Position;
using CardsAndMonsters.Features.Storage;
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
        private readonly IDuelLogService _duelLogService;
        private readonly ICardService _cardService;
        private readonly IBoardManagementService _boardManagementService;

        private bool disposedValue;

        public GameService(IDuelistFactory duelistFactory, IBattleService battleService,
            ITurnService turnService, IPhaseService phaseService, IPositionService positionService,
            IFakeOpponentService fakeOpponentService, IGameOverService gameOverService,
            IDuelLogService duelLogService, ICardService cardService, IBoardManagementService boardManagementService)
        {
            _duelistFactory = duelistFactory;
            _battleService = battleService;
            _turnService = turnService;
            _phaseService = phaseService;
            _positionService = positionService;
            _fakeOpponentService = fakeOpponentService;
            _gameOverService = gameOverService;
            _duelLogService = duelLogService;
            _cardService = cardService;
            _boardManagementService = boardManagementService;

            _phaseService.PhaseChanged += StateHasChanged;
        }

        public Board Board { get; set; }

        public Action OnAction { get; set; }

        public async Task<bool> CheckForExistingGame()
        {
            return await _boardManagementService.Load() != null;
        }

        public async Task ResumeGame()
        {
            Board = await _boardManagementService.Load();
            StateHasChanged();
        }

        public async Task StartGame()
        {
            Board = new(_duelistFactory.GetNewPlayer(), _duelistFactory.GetNewOpponent());

            Random rnd = new();
            var startingPlayer = rnd.Next(2) == 1 ? Board.Opponent : Board.Player;
            Board.CurrentTurn = new(startingPlayer);

            _duelLogService.AddNewEventLog(Event.GameStarted, startingPlayer);
            await _boardManagementService.Save(Board);

            await EnterPhase(Phase.Standby);

            for (int i = 0; i < AppConstants.HandSize; i++)
            {
                Board.Opponent.DrawCard();
                _duelLogService.AddNewEventLog(Event.DrawCard, Board.Opponent);

                Board.Player.DrawCard();
                _duelLogService.AddNewEventLog(Event.DrawCard, Board.Player);

                StateHasChanged();
                await Task.Delay(300);
            }
            await _boardManagementService.Save(Board);


            await _turnService.StartTurn(startingPlayer, false, Board);
            await _boardManagementService.Save(Board);

            if (!startingPlayer.Equals(Board.Player))
            {
                await FakeOpponentsTurn();
            }
        }

        public async Task ClearGame()
        {
            _gameOverService.ClearGameOverInfo();
            // need to find a better place than this as this will only delete old board if person clicks restart
            // if they leave and come back it'll ask to load the end of the last game each time.
            await _boardManagementService.Delete();
            StateHasChanged();
        }

        public async Task EnterPhase(Phase phase)
        {
            await _phaseService.EnterPhase(phase, Board);
            await _boardManagementService.Save(Board);
        }

        public void PlayCard(BaseCard card)
        {
            _cardService.PlayCard(card, Board);
        }

        public void PlayMonster(Monster monster)
        {
            _cardService.PlayMonster(monster);
        }

        public async Task PlayMonster(FieldPosition position)
        {
            _cardService.PlayMonster(position, Board);
            await _boardManagementService.Save(Board);
        }

        public async Task Attack(BattleInfo battleInfo)
        {
            _battleService.Attack(battleInfo);
            await _boardManagementService.Save(Board);
            _gameOverService.CheckForGameOver(Board);
        }

        public async Task SwitchPosition(Monster monster)
        {
            monster.FieldPosition = _positionService.NewPosition(monster.FieldPosition);
            _positionService.PositionSwitched(monster, Board);
            await _boardManagementService.Save(Board);
        }

        public async Task EndTurn()
        {
            await _turnService.EndTurn(Board);

            if (Board.CurrentTurn.Duelist.Equals(Board.Opponent))
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
