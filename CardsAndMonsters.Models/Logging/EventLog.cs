using CardsAndMonsters.Models.Base;
using CardsAndMonsters.Models.Enums;
using System;

namespace CardsAndMonsters.Models.Logging
{
    public class EventLog : BaseModel
    {
        public Event Event { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
