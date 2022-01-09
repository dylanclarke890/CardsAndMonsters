using CardsAndMonsters.Models.Base;
using CardsAndMonsters.Models.Enums;

namespace CardsAndMonsters.Models.Cards
{
    public abstract class BaseCard : BaseModel
    {
        public FieldPosition FieldPosition { get; set; }
    }
}
