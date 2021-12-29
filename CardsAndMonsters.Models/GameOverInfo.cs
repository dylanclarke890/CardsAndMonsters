using CardsAndMonsters.Models.Base;
using CardsAndMonsters.Models.Enums;
using CardsAndMonsters.Models.Logging;
using System.Collections.Generic;

namespace CardsAndMonsters.Models
{
    public class GameOverInfo : BaseModel
    {
        public GameOverInfo(
            Duelist player, 
            LossReason lossReason,
            IList<EventLog> eventLogs)
        {
            LosingPlayer = player;
            LossReason = lossReason;
            EventLogs = eventLogs;
        }

        public Duelist LosingPlayer { get; set; }

        public LossReason LossReason { get; set; }

        public IList<EventLog> EventLogs { get; set; }
    }
}
