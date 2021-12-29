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
    public class GameService : IGameService
    {
        private readonly IDuelistFactory _duelistFactory;
        private readonly IBattleService _battleService;
        private readonly ITurnService _turnService;
        private readonly IPhaseService _phaseService;
        private readonly IPositionService _positionService;
        private readonly IFakeOpponentService _fakeOpponentService;
        private readonly IGameOverService _gameOverService;

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

            _phaseService.EnterPhase(Phase.Standby, Board);
            StateHasChanged();

            for (int i = 0; i < AppConstants.HandSize; i++)
            {
                Board.Opponent.DrawCard();
                Board.Player.DrawCard();
                StateHasChanged();
                await Task.Delay(300);
            }

            _turnService.StartTurn(startingPlayer, false, Board);

            if (!startingPlayer.Equals(Board.Player))
            {
                _fakeOpponentService.FakeOpponentsTurn(Board);
            }
        }

        public void EnterPhase(Phase phase)
        {
            _phaseService.EnterPhase(phase, Board);
            StateHasChanged();
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
            PendingPlacement = monster;
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

        public void EndTurn()
        {
            _turnService.EndTurn(Board);

            if (Board.CurrentTurn.Player.Equals(Board.Opponent))
            {
                _fakeOpponentService.FakeOpponentsTurn(Board);
            }
            StateHasChanged();
        }

        private void StateHasChanged()
        {
            OnAction?.Invoke();
        }
    }
}
