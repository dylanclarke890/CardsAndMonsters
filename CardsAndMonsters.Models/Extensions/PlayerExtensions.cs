namespace CardsAndMonsters.Models.Extensions
{
    public static class PlayerExtensions
    {
        public static bool OutOfHealth(this Player player)
        {
            return player.HP <= 0;
        }
    }
}
