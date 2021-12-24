using System.Collections.Generic;

namespace CardsAndMonsters.Models
{
    public class Player : BaseModel
    {
        public Player()
        {
            HP = 4000;
            CardLimit = 6;
            CurrentHand = new List<BaseCard>();
            Deck = new List<BaseCard>();
        }

        public decimal HP { get; set; }

        public int CardLimit { get; set; }

        public IList<BaseCard> CurrentHand { get; set; }

        public IList<BaseCard> Deck {get; set;}
    }
}
