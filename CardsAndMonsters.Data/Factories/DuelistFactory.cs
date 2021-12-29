using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;

namespace CardsAndMonsters.Data.Factories
{
    public class DuelistFactory : IDuelistFactory
    {
        public Duelist GetNewPlayer()
        {
            var duelist = new Duelist("Player");
            AddCardsToDuelist(duelist);
            return duelist;
        }

        public Duelist GetNewOpponent()
        {
            var duelist = new Duelist("Opponent");
            AddCardsToDuelist(duelist);
            return duelist;
        }

        private static void AddCardsToDuelist(Duelist duelist)
        {
            duelist.Deck.Add(new Monster(1000, 800));
            duelist.Deck.Add(new Monster(1200, 300));
            duelist.Deck.Add(new Monster(1050, 100));
            duelist.Deck.Add(new Monster(1900, 1300));
            duelist.Deck.Add(new Monster(2000, 500));
            duelist.Deck.Add(new Monster(2000, 500));
            duelist.Deck.Add(new Monster(2000, 500));
            duelist.Deck.Add(new Monster(2000, 500));
            duelist.Deck.Add(new Monster(2000, 500));
        }
    }
}
