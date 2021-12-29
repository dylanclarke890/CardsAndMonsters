using CardsAndMonsters.Core;
using CardsAndMonsters.Features.Position;
using CardsAndMonsters.Features.Turn;
using CardsAndMonsters.Features.TurnPhase;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using System;

namespace CardsAndMonsters.Features.Opponent
{
    public class FakeOpponentService : IFakeOpponentService
    {
        private readonly IPhaseService _phaseService;
        private readonly ITurnService _turnService;
        private readonly IPositionService _positionService;
        public FakeOpponentService(IPhaseService phaseService,
            ITurnService turnService, IPositionService positionService)
        {
            _phaseService = phaseService;
            _turnService = turnService;
            _positionService = positionService;
        }


        public void FakeOpponentsTurn(Board board)
        {
            foreach (var monster in board.OpponentMonsters)
            {
                monster.FieldPosition = _positionService.NewPosition(monster.FieldPosition);
                board.CurrentTurn.MonsterState[monster.Id].AbleToSwitch = false;
            }

            Random rnd = new();

            var card = board.Opponent.CurrentHand[rnd.Next(board.Opponent.CurrentHand.Count)];
            if (card.IsType(typeof(Monster)) && board.OpponentMonsters.Count < AppConstants.FieldSize)
            {
                var monster = card as Monster;
                monster.FieldPosition = rnd.Next(2) == 1 ? FieldPosition.VerticalUp : FieldPosition.HorizontalDown;
                board.Opponent.PlayMonster(monster, board, board.CurrentTurn);

                if (board.TurnCount == 0)
                {
                    board.CurrentTurn.MonsterState[monster.Id].TimesAttacked = monster.AttacksPerTurn;
                }
            }

            _phaseService.EnterPhase(Phase.Battle, board);

            _turnService.EndTurn(board);
        }
    }
}
