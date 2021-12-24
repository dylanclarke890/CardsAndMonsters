using CardsAndMonsters.Models;

namespace CardsAndMonsters.Features
{
    public class PlayerBuilder
    {
        public static Player GetNewPlayer()
        {
            var player = new Player();
            return player;
        }

        public static Player GetNewOpponent()
        {
            var player = new Player();
            return player;
        }
    }
}
