using CardsAndMonsters.Models.Cards;
using System.Collections.Generic;

namespace CardsAndMonsters.Models
{
    public class FieldState
    {
        public FieldState(Duelist duelist)
        {
            Duelist = duelist;
            Graveyard = new List<BaseCard>();
            Monsters = new List<Monster>();
        }

        public Duelist Duelist { get; set; }

        public IList<BaseCard> Graveyard { get; set; }

        public IList<Monster> Monsters { get; set; }
    }
}
