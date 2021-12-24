namespace CardsAndMonsters.Models.Extensions
{
    public static class TurnStateExtensions
    {
        public static bool NormalSummonLimitReached(this TurnState turn)
        {
            return turn.NormalSummonedMonsters.Count == turn.NormalSummonLimit;
        }
    }
}
