using CardsAndMonsters.Core.Exceptions;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Enums;
using CardsAndMonsters.Models.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardsAndMonsters.Features.Logging
{
    public class DuelLogService : IDuelLogService
    {
        public DuelLogService()
        {
            EventLogs = new List<EventLog>();
        }

        private readonly IList<EventLog> EventLogs;

        public void AddNewEventLog(Event eventType, Duelist duelist)
        {
            if (duelist == null)
            {
                throw new GameArgumentException<Duelist>(nameof(duelist), duelist);
            }

            EventLog eventLog = new()
            {
                Event = eventType,
                CreatedAt = DateTime.UtcNow
            };
            eventLog.Description = eventType switch
            {
                Event.GameStarted => $"Game started with {duelist.Name} going first",
                Event.TurnChange => $"{duelist.Name} ended turn",
                Event.PhaseChange => $"{duelist.Name} changed phase",
                Event.DrawCard => $"{duelist.Name} drew a card",
                Event.MonsterPositionChange => $"{duelist.Name} changed the position of a monster",
                Event.PlayMonster => $"{duelist.Name} played a monster",
                Event.AttackDeclared => $"{duelist.Name} declared an attack",
                Event.MonsterDestroyed => $"{duelist.Name} destroyed a monster",
                Event.DamageTaken => $"{duelist.Name} took damage",
                Event.GameEnded => $"{duelist.Name} won the game",
                _ => throw new GameArgumentException<EventLog>(nameof(eventType), eventType),
            };

            EventLogs.Add(eventLog);
        }

        public IList<EventLog> GetEventLogs()
        {
            return EventLogs.Reverse().ToList();
        }
    }
}
