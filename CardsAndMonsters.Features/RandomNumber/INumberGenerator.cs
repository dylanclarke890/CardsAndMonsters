namespace CardsAndMonsters.Features.RandomNumber
{
    public interface INumberGenerator
    {
        int GetRandomNumber(int lessThan);
    }
}