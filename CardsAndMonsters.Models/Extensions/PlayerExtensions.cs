using System.Linq;

namespace CardsAndMonsters.Models.Extensions
{
    public static class PlayerExtensions
    {
        public static bool OutOfHealth(this Player player)
        {
            return player.HP <= 0;
        }

        public static bool DrawCard(this Player player)
        {
            if (!player.Deck.Any())
            {
                return false;
            }

            player.CurrentHand.Add(player.Deck.First());
            player.Deck.Remove(player.Deck.First());
            
            return true;
        }

        public static void PlayMonster(this Player player, Monster monster, Board board)
        {
            player.CurrentHand.Remove(monster);
            board.PlayerMonsters.Add(monster);
        }

        public static void TakeDamage(this Player player, decimal amount)
        {
            player.HP -= amount;
            if (player.HP < 0)
            {
                player.HP = 0;
            }
        }
    }
}
