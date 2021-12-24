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

        public int NormalSummonLimit { get; set; }

        public IList<Monster> NormalSummonedMonsters { get; set; }
    }
}
