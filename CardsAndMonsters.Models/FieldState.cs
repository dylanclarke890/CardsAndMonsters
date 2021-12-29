using CardsAndMonsters.Models.Base;
using CardsAndMonsters.Models.Cards;
using System.Collections.Generic;

namespace CardsAndMonsters.Models
{
    public class FieldState : BaseModel
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
