using CardsAndMonsters.Models;

namespace CardsAndMonsters.Features
{
    public class PlayerBuilder
    {
        public static Player GetNewPlayer()
        {
            var player = new Player();
            AddCardsToPlayer(player);
            return player;
        }

        public static Player GetNewOpponent()
        {
            var player = new Player();
            AddCardsToPlayer(player);
            return player;
        }

        public static void AddCardsToPlayer(Player player)
        {
            player.Deck.Add(new Monster(1000, 800));
            player.Deck.Add(new Monster(1200, 300));
            player.Deck.Add(new Monster(1050, 100));
            player.Deck.Add(new Monster(1900, 1300));
            player.Deck.Add(new Monster(2000, 500));
        }
    }
}
