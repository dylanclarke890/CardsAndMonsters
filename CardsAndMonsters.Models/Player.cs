using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CardsAndMonsters.Models
{
    public class Player : BaseModel, IEqualityComparer<Player>
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

        public bool Equals(Player x, Player y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] Player obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
