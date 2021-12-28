using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;

namespace CardsAndMonsters.Features.Position
{
    public interface IPositionService
    {
        FieldPosition NewPosition(FieldPosition old);
        void PositionSwitched(Monster monster, Board board);
    }
}