using CardsAndMonsters.Models.Enums;
using System.Collections.Generic;

namespace CardsAndMonsters.Models
{
    public class TurnState : BaseModel
    {
        public TurnState()
        {
            NormalSummonLimit = 1;
            NormalSummonedMonsters = new List<Monster>();
        }

        public Phase Phase { get; set; }

        public int NormalSummonLimit { get; set; }

        public IList<Monster> NormalSummonedMonsters { get; set; }

        public bool NormalSummonLimitReached()
        {
            return NormalSummonedMonsters.Count == NormalSummonLimit;
        }
    }
}
