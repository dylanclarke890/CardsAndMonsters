using CardsAndMonsters.Models;

namespace CardsAndMonsters.Features.Turn
{
    public interface ITurnService
    {
        void EndTurn(Board board);
        void StartTurn(Duelist player, bool drawCard, Board board);
    }
}