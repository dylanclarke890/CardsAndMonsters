using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;

namespace CardsAndMonsters.Features
{
    public class PlayerBuilder
    {
        public static Duelist GetNewPlayer()
        {
            var player = new Duelist();
            AddCardsToPlayer(player);
            return player;
        }

        public static Duelist GetNewOpponent()
        {
            var player = new Duelist();
            AddCardsToPlayer(player);
            return player;
        }

        public static void AddCardsToPlayer(Duelist player)
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
