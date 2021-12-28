using CardsAndMonsters.Models.Base;
using CardsAndMonsters.Models.Enums;
using System;

namespace CardsAndMonsters.Models.Cards
{
    public abstract class BaseCard : BaseModel
    {
        public FieldPosition FieldPosition { get; set; }

        public bool IsType(Type type)
        {
            return GetType() == type;
        }
    }
}
