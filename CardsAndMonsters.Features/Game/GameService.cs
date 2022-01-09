using CardsAndMonsters.Core;
using CardsAndMonsters.Core.Exceptions;
using CardsAndMonsters.Data.Factories;
using CardsAndMonsters.Features.Battle;
using CardsAndMonsters.Features.Card;
using CardsAndMonsters.Features.GameOver;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Features.Opponent;
using CardsAndMonsters.Features.Position;
using CardsAndMonsters.Features.RandomNumber;
using CardsAndMonsters.Features.Storage;
using CardsAndMonsters.Features.Turn;
using CardsAndMonsters.Features.TurnPhase;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CardsAndMonsters.Features.Game
{
    public class GameService : IGameService, IDisposable
    {
        private readonly IBattleService _battleService;
        private readonly IBoardManagementService _boardManagementService;
        private readonly ICardService _cardService;
        private readonly IDuelistFactory _duelistFactory;
        private readonly IDuelLogService _duelLogService;
        private readonly IFakeOpponentService _fakeOpponentService;
        private readonly IGameOverService _gameOverService;
        private readonly INumberGenerator _numberGenerator;
        private readonly IPhaseService _phaseService;
        private readonly IPositionService _positionService;
        private readonly ITurnService _turnService;

        private bool disposedValue;

        public GameService(IBattleService battleService, IBoardManagementService boardManagementService,
            ICardService cardService, IDuelistFactory duelistFactory, IDuelLogService duelLogService,
            IFakeOpponentService fakeOpponentService, IGameOverService gameOverService, INumberGenerator numberGenerator,
            IPhaseService phaseService, IPositionService positionService, ITurnService turnService)
        {
            _battleService = battleService;
            _boardManagementService = boardManagementService;
            _cardService = cardService;
            _duelistFactory = duelistFactory;
            _duelLogService = duelLogService;
            _fakeOpponentService = fakeOpponentService;
            _gameOverService = gameOverService;
            _numberGenerator = numberGenerator;
            _phaseService = phaseService;
            _positionService = positionService;
            _turnService = turnService;

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
            await _turnService.ResumeTurn(Board);

            if (Board.TurnCount == 0 && !Board.Player.CurrentHand.Any() && !Board.Opponent.CurrentHand.Any())
            {
                await DrawInitialCardsAsync();
            }

            if (Board.CurrentTurn.Duelist.Equals(Board.Opponent))
            {
                await _fakeOpponentService.ResumePhase(Board);
            }
        }

        public async Task NewGame()
        {
            await GetNewBoardAsync();
            await _turnService.StartTurn(Board.CurrentTurn.Duelist, false, Board);
            await DrawInitialCardsAsync();

            if (Board.CurrentTurn.Duelist.Equals(Board.Opponent))
            {
                await FakeOpponentsTurn();
            }
        }

        private async Task GetNewBoardAsync()
        {
            Board = new(_duelistFactory.GetNewPlayer(), _duelistFactory.GetNewOpponent());

            var startingPlayer = _numberGenerator.GetRandom(2) == 1 ? Board.Opponent : Board.Player;
            Board.CurrentTurn = new(startingPlayer);

            _duelLogService.AddNewEventLog(Event.GameStarted, startingPlayer);
            await _boardManagementService.Save(Board);
        }

        private async Task DrawInitialCardsAsync()
        {
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
        }

        public async Task ClearGame()
        {
            _gameOverService.ClearGameOverInfo();
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
            if (card == null)
            {
                throw new GameArgumentException<BaseCard>(nameof(card), card);
            }

            _cardService.PlayCard(card, Board);
        }

        public void PlayMonster(Monster monster)
        {
            if (monster == null)
            {
                throw new GameArgumentException<Monster>(nameof(monster), monster);
            }

            _cardService.PlayMonster(monster);
        }

        public async Task PlayMonster(FieldPosition position)
        {
            if (position is FieldPosition.VerticalDown)
            {
                throw new GameArgumentException<Monster>(nameof(position), position);
            }

            _cardService.PlayMonster(position, Board);
            await _boardManagementService.Save(Board);
        }

        public async Task Attack(BattleInfo battleInfo)
        {
            if (battleInfo == null)
            {
                throw new GameArgumentException<BattleInfo>(nameof(battleInfo), battleInfo);
            }

            _battleService.Attack(battleInfo);
            await _boardManagementService.Save(Board);
            await _gameOverService.CheckForGameOver(Board);
        }

        public async Task SwitchPosition(Monster monster)
        {
            if (monster == null)
            {
                throw new GameArgumentException<Monster>(nameof(monster), monster);
            }

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
