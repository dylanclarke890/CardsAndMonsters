using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Enums;
using CardsAndMonsters.Models.Logging;
using System.Collections.Generic;

namespace CardsAndMonsters.Features.Logging
{
    public interface IDuelLogService
    {
        void AddNewEventLog(Event eventType, Duelist duelist);
        IList<EventLog> GetEventLogs();
    }
}