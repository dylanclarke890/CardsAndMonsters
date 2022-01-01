using CardsAndMonsters.Models.Base;
using CardsAndMonsters.Models.Cards;

namespace CardsAndMonsters.Models.Turns
{
    public class MonsterTurnState : BaseModel
    {
        public Monster Monster { get; set; }

        public bool AbleToSwitch { get; set; }

        public int TimesAttacked { get; set; }

        public bool Destroyed { get; set; }
    }
}
