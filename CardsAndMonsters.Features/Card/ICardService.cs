using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;

namespace CardsAndMonsters.Features.Card
{
    public interface ICardService
    {
        bool ChoosingFieldPosition { get; set; }
        BaseCard PendingPlacement { get; set; }

        void PlayCard(BaseCard card, Board board);
        void PlayMonster(FieldPosition position, Board board);
        void PlayMonster(Monster monster);
    }
}