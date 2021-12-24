using System.Linq;

namespace CardsAndMonsters.Models.Extensions
{
    public static class PlayerExtensions
    {
        public static bool OutOfHealth(this Player player)
        {
            return player.HP <= 0;
        }

        public static void DrawCard(this Player player)
        {
            player.CurrentHand.Add(player.Deck.First());
            player.Deck.Remove(player.Deck.First());
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
