using CardsAndMonsters.Models;

namespace CardsAndMonsters.Data.Factories
{
    public interface IDuelistFactory
    {
        Duelist GetNewOpponent();
        Duelist GetNewPlayer();
    }
}