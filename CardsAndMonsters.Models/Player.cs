using System.Collections.Generic;

namespace CardsAndMonsters.Models
{
    public class Player : BaseModel
    {
        public decimal HP { get; set; }

        public int CardLimit { get; set; }

        public IList<BaseCard> CurrentHand { get; set; }

        public IList<BaseCard> Cards {get; set;}
    }
}
