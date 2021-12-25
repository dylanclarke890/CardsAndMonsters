using CardsAndMonsters.Models.Enums;

namespace CardsAndMonsters.Models
{
    public abstract class BaseCard : BaseModel
    {
        public FieldPosition FieldPosition { get; set; }
    }
}
