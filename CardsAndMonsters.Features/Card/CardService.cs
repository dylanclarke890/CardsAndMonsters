using CardsAndMonsters.Core;
using CardsAndMonsters.Core.Exceptions;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;

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
            if (card == null)
            {
                throw new GameArgumentException<BaseCard>(nameof(card), card);
            }
            if (board == null)
            {
                throw new GameArgumentException<BaseCard>(nameof(board), board);
            }

            if (card.IsType(typeof(Monster)))
            {
                var monster = card as Monster;
                if (board.Player.Equals(board.CurrentTurn.Duelist))
                {
                    if (board.PlayerField.Monsters.Count == AppConstants.FieldSize || board.CurrentTurn.NormalSummonLimitReached())
                    {
                        string error = board.CurrentTurn.NormalSummonLimitReached() ? "normal summon limit reached" : "field is full";
                        throw new IncorrectMoveException($"Tried playing a monster when {error}");
                    }

                    PlayMonster(monster);
                }
            }
        }

        public void PlayMonster(Monster monster)
        {
            ChoosingFieldPosition = true;
            PendingPlacement = monster ?? throw new GameArgumentException<Monster>(nameof(monster), monster);
        }

        public void PlayMonster(FieldPosition position, Board board)
        {
            if (position is FieldPosition.VerticalDown)
            {
                throw new GameArgumentException<Monster>(nameof(position), position);
            }
            if (board == null)
            {
                throw new GameArgumentException<Monster>(nameof(board), board);
            }

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
