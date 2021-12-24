using System.Collections.Generic;

namespace CardsAndMonsters.Models
{
    public class Board : BaseModel
    {
        public Board()
        {
            OpponentMonsters = new List<Monster>();
            PlayerMonsters = new List<Monster>();
        }

        public IList<Monster> OpponentMonsters { get; set; }
        public IList<Monster> PlayerMonsters { get; set; }
    }
}
