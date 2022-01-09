namespace CardsAndMonsters.Features.RandomNumber
{
    public interface INumberGenerator
    {
        int GetRandom(int lessThan);
    }
}