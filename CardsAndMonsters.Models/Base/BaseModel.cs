using System;

namespace CardsAndMonsters.Models.Base
{
    public abstract class BaseModel
    {
        public BaseModel()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }
}
