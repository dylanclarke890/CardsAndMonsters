using System;

namespace CardsAndMonsters.Features.RandomNumber
{
    public class NumberGenerator : INumberGenerator
    {
        private readonly Random _random;

        public NumberGenerator()
        {
            _random = new();
        }

        public int GetRandom(int lessThan)
        {
            return _random.Next(lessThan);
        }
    }
}
