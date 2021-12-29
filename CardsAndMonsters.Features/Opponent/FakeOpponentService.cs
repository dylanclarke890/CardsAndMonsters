﻿using CardsAndMonsters.Core;
using CardsAndMonsters.Features.Position;
using CardsAndMonsters.Features.Turn;
using CardsAndMonsters.Features.TurnPhase;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using System;
using System.Threading.Tasks;

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

        public async Task FakeOpponentsTurn(Board board)
        {
            foreach (var monster in board.OpponentField.Monsters)
            {
                monster.FieldPosition = _positionService.NewPosition(monster.FieldPosition);
                board.CurrentTurn.MonsterState[monster.Id].AbleToSwitch = false;
            }

            Random rnd = new();

            var card = board.Opponent.CurrentHand[rnd.Next(board.Opponent.CurrentHand.Count)];
            if (card.IsType(typeof(Monster)) && board.OpponentField.Monsters.Count < AppConstants.FieldSize)
            {
                var monster = card as Monster;
                monster.FieldPosition = rnd.Next(2) == 1 ? FieldPosition.VerticalUp : FieldPosition.HorizontalDown;
                board.Opponent.PlayMonster(monster, board, board.CurrentTurn);

                if (board.TurnCount == 0)
                {
                    board.CurrentTurn.MonsterState[monster.Id].TimesAttacked = monster.AttacksPerTurn;
                }
            }

            await _phaseService.EnterPhase(Phase.Battle, board);

            await _turnService.EndTurn(board);
        }
    }
}
