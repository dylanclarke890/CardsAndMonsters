using CardsAndMonsters.Models;

namespace CardsAndMonsters.Features.Opponent
{
    public interface IFakeOpponentService
    {
        void FakeOpponentsTurn(Board board);
    }
}