using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;

namespace CardsAndMonsters.Data.Factories
{
    public class DuelistFactory : IDuelistFactory
    {
        public Duelist GetNewPlayer()
        {
            var duelist = new Duelist();
            AddCardsToDuelist(duelist);
            return duelist;
        }

        public Duelist GetNewOpponent()
        {
            var duelist = new Duelist();
            AddCardsToDuelist(duelist);
            return duelist;
        }

        private static void AddCardsToDuelist(Duelist player)
        {
            player.Deck.Add(new Monster(1000, 800));
            player.Deck.Add(new Monster(1200, 300));
            player.Deck.Add(new Monster(1050, 100));
            player.Deck.Add(new Monster(1900, 1300));
            player.Deck.Add(new Monster(2000, 500));
            player.Deck.Add(new Monster(2000, 500));
            player.Deck.Add(new Monster(2000, 500));
            player.Deck.Add(new Monster(2000, 500));
            player.Deck.Add(new Monster(2000, 500));
        }
    }
}
