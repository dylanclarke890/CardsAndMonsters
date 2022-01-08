using CardsAndMonsters.Core;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using System;

namespace CardsAndMonsters.Features.Card
{
    public class CardService : ICardService
    {
        private readonly IDuelLogService _duelLogService;

        public CardService(IDuelLogService duelLogService)
        {
            _duelLogService = duelLogService;
        }

        public bool ChoosingFieldPosition { get; set; }

        public BaseCard PendingPlacement { get; set; }

        public void PlayCard(BaseCard card, Board board)
        {
            if (card.IsType(typeof(Monster)))
            {
                var monster = card as Monster;
                if (board.Player.Equals(board.CurrentTurn.Duelist))
                {
                    if (board.PlayerField.Monsters.Count == AppConstants.FieldSize || board.CurrentTurn.NormalSummonLimitReached())
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
            PendingPlacement = monster ?? throw new ArgumentException("Needs a monster to be able to select placement.");
        }

        public void PlayMonster(FieldPosition position, Board board)
        {
            ChoosingFieldPosition = false;
            var monster = PendingPlacement as Monster;
            monster.FieldPosition = position;

            board.Player.PlayMonster(monster, board, board.CurrentTurn);
            _duelLogService.AddNewEventLog(Event.PlayMonster, board.Player);

            if (board.TurnCount == 0)
            {
                board.CurrentTurn.MonsterState[monster.Id].TimesAttacked = monster.AttacksPerTurn;
            }
            PendingPlacement = null;
        }
    }
}
