using System.Collections.Generic;

namespace CardsAndMonsters.Models
{
    public class Board : BaseModel
    {
        public Board(Player player, Player opponent)
        {
            Player = player;
            Opponent = opponent;
            OpponentMonsters = new List<Monster>();
            PlayerMonsters = new List<Monster>();
        }

        public Player Player { get; set; }
        
        public Player Opponent { get; set; }
        
        public IList<Monster> OpponentMonsters { get; set; }
        
        public IList<Monster> PlayerMonsters { get; set; }
    }
}
